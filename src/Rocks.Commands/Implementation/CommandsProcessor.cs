using System;
using System.Diagnostics;
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

		[DebuggerStepThrough]
		public TResult Execute<TResult> (ICommand<TResult> command)
		{
			Validate (command);

			var handler = this.GetHandler (command);

			return handler.Execute ((dynamic) command);
		}


		[DebuggerStepThrough]
		public async Task<TResult> ExecuteAsync<TResult> (IAsyncCommand<TResult> command, CancellationToken cancellationToken = default (CancellationToken))
		{
			Validate (command);

			var handler = this.GetAsyncHandler (command);

			return await handler.ExecuteAsync ((dynamic) command, cancellationToken)
			                    .ConfigureAwait (false);
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


		private dynamic GetHandler<TResult> (ICommand<TResult> command)
		{
			var type = typeof (ICommandHandler<,>).MakeGenericType (command.GetType (), typeof (TResult));

			dynamic handler = this.commandHandlerFactory.GetCommandHandler (type);

			return handler;
		}


		private dynamic GetAsyncHandler<TResult> (IAsyncCommand<TResult> command)
		{
			var type = typeof (IAsyncCommandHandler<,>).MakeGenericType (command.GetType (), typeof (TResult));

			dynamic handler = this.commandHandlerFactory.GetCommandHandler (type);

			return handler;
		}

		#endregion
	}
}