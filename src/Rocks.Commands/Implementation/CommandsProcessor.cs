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
		#region Private fields

		private readonly ICommandHandlerFactory commandHandlerFactory;

		#endregion

		#region Construct

		public CommandsProcessor ([NotNull] ICommandHandlerFactory commandHandlerFactory)
		{
			if (commandHandlerFactory == null)
				throw new ArgumentNullException ("commandHandlerFactory");

			this.commandHandlerFactory = commandHandlerFactory;
		}

		#endregion

		#region ICommandsProcessor Members

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

			var handler = this.GetHandlerForArbitraryCommand (command);

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

			var handler = this.GetHandlerForArbitraryCommand (command);

			return await handler.ExecuteAsync ((dynamic) command, cancellationToken)
			                    .ConfigureAwait (false);
		}


		/// <summary>
		///     Returns the list of <see cref="IDecorator{TCommand,TResult}" /> decorators instances
		///     that registered for a given command.
		/// </summary>
		public IList<IDecorator<TCommand, TResult>> GetAllDecorators<TCommand, TResult> () where TCommand : ICommand<TResult>
		{
			var result = new List<IDecorator<TCommand, TResult>> ();

			var handler = this.GetHandler (typeof (ICommandHandler<,>), typeof (TCommand), typeof (TResult));

			var decorator = handler as IDecorator<TCommand, TResult>;
			while (decorator != null)
			{
				result.Add (decorator);
				decorator = decorator.Decorated as IDecorator<TCommand, TResult>;
			}

			return result;
		}


		/// <summary>
		///     Returns the list of <see cref="IAsyncDecorator{TCommand,TResult}" /> async decorators instances
		///     that registered for a given command.
		/// </summary>
		public IList<IAsyncDecorator<TCommand, TResult>> GetAllAsyncDecorators<TCommand, TResult> () where TCommand : IAsyncCommand<TResult>
		{
			var result = new List<IAsyncDecorator<TCommand, TResult>> ();

			var handler = this.GetHandler (typeof (IAsyncCommandHandler<,>), typeof (TCommand), typeof (TResult));

			var decorator = handler as IAsyncDecorator<TCommand, TResult>;
			while (decorator != null)
			{
				result.Add (decorator);
				decorator = decorator.Decorated as IAsyncDecorator<TCommand, TResult>;
			}

			return result;
		}

		#endregion

		#region Private methods

		private static void Validate (object command)
		{
			if (command == null)
				throw new ArgumentNullException ("command");

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


		private dynamic GetHandlerForArbitraryCommand (ICommand command)
		{
			var type = GetCommandHandlerType (command, typeof (ICommand<>), typeof (ICommandHandler<,>));

			dynamic handler = this.commandHandlerFactory.GetCommandHandler (type);

			return handler;
		}


		private dynamic GetHandlerForArbitraryCommand (IAsyncCommand command)
		{
			var type = GetCommandHandlerType (command, typeof (IAsyncCommand<>), typeof (IAsyncCommandHandler<,>));

			dynamic handler = this.commandHandlerFactory.GetCommandHandler (type);

			return handler;
		}


		private static Type GetCommandHandlerType (object command, Type commandOpenGenericType, Type commandHandlerOpenGenericType)
		{
			var command_type = command.GetType ();

			var command_generic_type = command_type.GetInterfaces ()
			                                       .FirstOrDefault (i => i.IsGenericType &&
			                                                             i.GetGenericTypeDefinition () == commandOpenGenericType);

			if (command_generic_type == null)
				throw new InvalidOperationException (string.Format ("The command {0} does not implements {1}.", command_type, commandOpenGenericType));

			var type = commandHandlerOpenGenericType.MakeGenericType (command_type, command_generic_type.GenericTypeArguments[0]);

			return type;
		}

		#endregion
	}
}