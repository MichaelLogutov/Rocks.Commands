using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Commands.Implementation
{
	[UsedImplicitly]
	internal class CommandsProcessor : ICommandsProcessor
	{
		private readonly ICommandHandlerFactory commandHandlerFactory;


		public CommandsProcessor ([NotNull] ICommandHandlerFactory commandHandlerFactory)
		{
			if (commandHandlerFactory == null)
				throw new ArgumentNullException (nameof(commandHandlerFactory));

			this.commandHandlerFactory = commandHandlerFactory;
		}


		/// <summary>
		///     Executes specified <paramref name="command" />.
		/// </summary>
		/// <typeparam name="TResult">Type of the command result.</typeparam>
		/// <param name="command">Command instance to execute.</param>
		[DebuggerStepThrough]
		public TResult Execute<TResult> (ICommand<TResult> command)
		{
			Validate (command);

			var handler = this.GetHandler (typeof (ICommandHandler<,>), command.GetType (), typeof (TResult));

			return handler.Execute ((dynamic) command);
		}


		/// <summary>
		///     Executes arbitrary <paramref name="command" />.
		/// </summary>
		/// <param name="command">Command instance to execute.</param>
		[DebuggerStepThrough]
		public object Execute (ICommand command)
		{
			Validate (command);

			var handler = this.GetHandlerForArbitraryCommand (command.GetType ());

			return handler.Execute ((dynamic) command);
		}


		/// <summary>
		///     Executes specified <paramref name="command" /> asynchronously
		///     using <see cref="IAsyncCommandHandler{TCommand,TResult}"/> (needs to be registered).
		/// </summary>
		/// <typeparam name="TResult">Type of the command result.</typeparam>
		/// <param name="command">Command instance to execute.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		[DebuggerStepThrough]
		public Task<TResult> ExecuteAsync<TResult> (IAsyncCommand<TResult> command, CancellationToken cancellationToken = default (CancellationToken))
		{
			Validate (command);

			var handler = this.GetHandler (typeof (IAsyncCommandHandler<,>), command.GetType (), typeof (TResult));

			return handler.ExecuteAsync ((dynamic) command, cancellationToken);
		}


		/// <summary>
		///     Executes arbitrary <paramref name="command" />.
		/// </summary>
		/// <param name="command">Command instance to execute.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		[DebuggerStepThrough]
		public async Task<object> ExecuteAsync (IAsyncCommand command, CancellationToken cancellationToken = default (CancellationToken))
		{
			Validate (command);

			var handler = this.GetAsyncHandlerForArbitraryCommand (command.GetType ());

			return await handler.ExecuteAsync ((dynamic) command, cancellationToken)
			                    .ConfigureAwait (false);
		}


		/// <summary>
		///     Returns the list of <see cref="IDecorator" /> decorators instances
		///     that registered for a given command.
		/// </summary>
		public IList<IDecorator> GetAllDecorators<TCommand> () where TCommand : ICommand
		{
			var handler = this.GetHandlerForArbitraryCommand (typeof (TCommand));

			var result = GetDecoratorsList (handler);

			return result;
		}


		/// <summary>
		///     Returns the list of <see cref="IDecorator" /> decorators instances
		///     that registered for a given command.
		/// </summary>
		public IList<IDecorator> GetAllAsyncDecorators<TCommand> () where TCommand : IAsyncCommand
		{
			var handler = this.GetAsyncHandlerForArbitraryCommand (typeof (TCommand));

			var result = GetDecoratorsList (handler);

			return result;
		}


		private static void Validate (object command)
		{
			if (command == null)
				throw new ArgumentNullException (nameof(command));

			var validatable_command = command as IValidatableCommand;
			if (validatable_command != null)
				validatable_command.Validate ();
		}


		private dynamic GetHandler (Type commandHandlerType, Type commandType, Type commandReturnType)
		{
			var type = commandHandlerType.MakeGenericType (commandType, commandReturnType);

			dynamic handler = this.commandHandlerFactory.GetCommandHandler (type);

			return handler;
		}


		private dynamic GetHandlerForArbitraryCommand (Type commandType)
		{
			var type = GetCommandHandlerType (commandType, typeof (ICommand<>), typeof (ICommandHandler<,>));

			dynamic handler = this.commandHandlerFactory.GetCommandHandler (type);

			return handler;
		}


		private dynamic GetAsyncHandlerForArbitraryCommand (Type commandType)
		{
			var type = GetCommandHandlerType (commandType, typeof (IAsyncCommand<>), typeof (IAsyncCommandHandler<,>));

			dynamic handler = this.commandHandlerFactory.GetCommandHandler (type);

			return handler;
		}


		private static Type GetCommandHandlerType (Type commandType, Type commandOpenGenericType, Type commandHandlerOpenGenericType)
		{
			var command_generic_type = commandType.GetInterfaces ()
			                                      .FirstOrDefault (i => i.IsGenericType &&
			                                                            i.GetGenericTypeDefinition () == commandOpenGenericType);

			if (command_generic_type == null)
				throw new InvalidOperationException (string.Format ("The command {0} does not implements {1}.", commandType, commandOpenGenericType));

			var type = commandHandlerOpenGenericType.MakeGenericType (commandType, command_generic_type.GenericTypeArguments[0]);

			return type;
		}


		private static List<IDecorator> GetDecoratorsList (object handler)
		{
			var result = new List<IDecorator> ();

			var decorator = handler as IDecorator;
			while (decorator != null)
			{
				result.Add (decorator);
				decorator = decorator.Decorated as IDecorator;
			}

			return result;
		}
	}
}