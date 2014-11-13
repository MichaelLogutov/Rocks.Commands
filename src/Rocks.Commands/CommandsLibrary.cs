using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Rocks.Commands.Exceptions;
using Rocks.Commands.Implementation;
using SimpleInjector;
using SimpleInjector.Extensions;

namespace Rocks.Commands
{
	/// <summary>
	///     A static facade of Rocks.Commands library.
	///     Call <see cref="Setup" />
	///     method to initialize the library.
	/// </summary>
	public static class CommandsLibrary
	{
		#region Private fields

		private static Lifestyle defaultLifestyle;

		#endregion

		#region Public properties

		/// <summary>
		///     Dependency injection container that holds registration of all commands, handlers and implementation
		///     of all interfaces of Rocks.Commands library. Use it to get all instances of requried types from the library.
		/// </summary>
		public static Container Container { get; set; }

		/// <summary>
		///     An instance of commands processor that can be used statically to execute commands.
		///     Note: a direct usage of <see cref="ICommandsProcessor" /> via constructor injection instead of
		///     using this static field is preferred way of working with the library.
		/// </summary>
		public static ICommandsProcessor CommandsProcessor { get; set; }

		#endregion

		#region Static methods

		/// <summary>
		///     Perform initial setup of commands library by finding all commands handlers in <paramref name="assemblies" />
		///     and registering them in <paramref name="container" /> with specified <paramref name="lifestyle" />.
		/// </summary>
		/// <param name="container">Container that will be used for registration. If null the inner container will be created.</param>
		/// <param name="lifestyle">Default lifestyle for command handlers. If null the transient lifestyle will be used.</param>
		/// <param name="verifyAllCommandsHasHandlers">
		///     If true will throws <see cref="CommandHandlerNotFoundException" /> for any
		///     command found without appropriate command handler.
		/// </param>
		/// <param name="assemblies">
		///     A list of assemblies to scan for <see cref="ICommandHandler{TCommand,TResult}" />
		///     implementations. If null or empty the calling assembly will be used.
		/// </param>
		public static void Setup (Container container = null,
		                          Lifestyle lifestyle = null,
		                          bool verifyAllCommandsHasHandlers = true,
		                          params Assembly[] assemblies)
		{
			Container = container ?? new Container (new ContainerOptions { AllowOverridingRegistrations = true });
			defaultLifestyle = lifestyle ?? Lifestyle.Transient;

			if (assemblies == null || assemblies.Length == 0)
				assemblies = new[] { Assembly.GetCallingAssembly () };

			var handler_factory = new SimpleInjectorCommandHandlerFactory (Container);
			Container.RegisterSingle (handler_factory);

			CommandsProcessor = new CommandsProcessor (handler_factory);
			Container.RegisterSingle (CommandsProcessor);

			RegisterAllCommandHandlers (Container, lifestyle, verifyAllCommandsHasHandlers, assemblies);
		}


		/// <summary>
		///     Finds all command handlers in <paramref name="assemblies" /> and register them in <paramref name="container" />
		///     with specified <paramref name="lifestyle" />.
		///     Note: if your command handlers implementations are internal then add InternalsVisibleTo for Rocks.Commands library
		///     or execution will fail.
		/// </summary>
		/// <param name="container">
		///     Container that will be used for registration. If null <see cref="Container" /> will be used
		///     (should be initialized with calling <see cref="Setup" /> method before).
		/// </param>
		/// <param name="lifestyle">
		///     Lifestyle of the registered command handlers.
		///     If null then lifestyle passed to previous call to <see cref="Setup" /> method will be used.
		/// </param>
		/// <param name="verifyAllCommandsHasHandlers">
		///     If true will throws <see cref="CommandHandlerNotFoundException" /> for any
		///     command found without appropriate command handler.
		/// </param>
		/// <param name="assemblies">
		///     A list of assemblies to scan for <see cref="ICommandHandler{TCommand,TResult}" />
		///     implementations. If null or empty the calling assembly will be used.
		/// </param>
		public static void RegisterAllCommandHandlers (Container container = null,
		                                               Lifestyle lifestyle = null,
		                                               bool verifyAllCommandsHasHandlers = true,
		                                               params Assembly[] assemblies)
		{
			container = container ?? Container;
			lifestyle = lifestyle ?? defaultLifestyle;

			if (container == null)
				throw new ArgumentNullException ("container", "Parameter 'container' is null and Setup method was not called before");

			if (lifestyle == null)
				throw new ArgumentNullException ("lifestyle", "Parameter 'lifestyle' is null and Setup method was not called before");

			if (assemblies == null || assemblies.Length == 0)
				assemblies = new[] { Assembly.GetCallingAssembly () };

			container.RegisterManyForOpenGeneric (typeof (ICommandHandler<,>), AccessibilityOption.AllTypes, lifestyle, assemblies);
			container.RegisterManyForOpenGeneric (typeof (IAsyncCommandHandler<,>), AccessibilityOption.AllTypes, lifestyle, assemblies);

			if (verifyAllCommandsHasHandlers)
				VerifyAllCommandsHasHandlers (container, assemblies);
		}


		/// <summary>
		/// Throws <see cref="CommandHandlerNotFoundException" /> for any command found without appropriate command handler.
		/// </summary>
		/// <param name="container">Container with all registered command handlers.</param>
		/// <param name="assemblies">Assemblies with commands to be checked.</param>
		public static void VerifyAllCommandsHasHandlers ([NotNull] Container container, params Assembly[] assemblies)
		{
			if (container == null)
				throw new ArgumentNullException ("container");

			var command_generic_type = typeof (ICommand<>);
			var command_handler_generic_type = typeof (ICommandHandler<,>);
			var registered_types = container.GetCurrentRegistrations ().Select (x => x.Registration.ImplementationType).ToList ();

			var registered_command_handlers_types = new HashSet<Type>
				(registered_types
					 .Where (t => t.IsClass && !t.IsAbstract)
					 .SelectMany (t => t.GetInterfaces ()
					                    .Where (i => i.IsGenericType && i.GetGenericTypeDefinition () == command_handler_generic_type))
					 .Distinct ());


			foreach (var command_type in assemblies.SelectMany (a => a.GetTypes ().Where (x => x.IsClass && !x.IsAbstract))
			                                       .Distinct ())
			{
				var command_interfaces = command_type.GetInterfaces ()
				                                     .Where (i => i.IsGenericType && i.GetGenericTypeDefinition () == command_generic_type)
				                                     .ToList ();

				if (!command_interfaces.Any ())
					continue;

				foreach (var command_interface in command_interfaces)
				{
					var command_result_type = command_interface.GetGenericArguments ()[0];
					var command_handler_type = command_handler_generic_type.MakeGenericType (command_type, command_result_type);

					if (!registered_command_handlers_types.Contains (command_handler_type))
						throw new CommandHandlerNotFoundException (command_type, command_result_type);
				}
			}
		}


		/// <summary>
		///     Register command decorator.
		/// </summary>
		/// <param name="decoratorType">
		///     Decorator type. Can be open generic type, for example typeof (MyCommandDecorator&lt;,&gt;).
		/// </param>
		/// <param name="container">
		///     Container that will be used for registration. If null <see cref="Container" /> will be used
		///     (should be initialized with calling <see cref="Setup" /> method before).
		/// </param>
		/// <param name="lifestyle">
		///     Lifestyle of the registered decorator.
		///     Note that it change the lifestyle of all decorated commands.
		///     If null then lifestyle passed to previous call to <see cref="Setup" /> method will be used.
		/// </param>
		public static void RegisterCommandsDecorator ([NotNull] Type decoratorType, Container container = null, Lifestyle lifestyle = null)
		{
			if (decoratorType == null)
				throw new ArgumentNullException ("decoratorType");

			container = container ?? Container;
			lifestyle = lifestyle ?? defaultLifestyle;

			if (container == null)
				throw new ArgumentNullException ("container", "Parameter 'container' is null and Setup method was not called before");

			if (lifestyle == null)
				throw new ArgumentNullException ("lifestyle", "Parameter 'lifestyle' is null and Setup method was not called before");

			container.RegisterDecorator (typeof (ICommandHandler<,>), decoratorType, lifestyle);
		}

		#endregion
	}
}