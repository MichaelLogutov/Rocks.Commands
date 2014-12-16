namespace Rocks.Commands.Tests.Decorators.Sync.Commands
{
	internal class TestDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>,
	                                                  IDecorator<TCommand, TResult>
		where TCommand : ICommand<TResult>, IDecoratableCommand
	{
		private readonly ICommandHandler<TCommand, TResult> decorated;


		public TestDecorator (ICommandHandler<TCommand, TResult> decorated)
		{
			this.decorated = decorated;
		}


		public TResult Execute (TCommand command)
		{
			var result = this.decorated.Execute (command);
			command.Number++;
			return result;
		}


		/// <summary>
		///     A decorated command handler.
		/// </summary>
		public ICommandHandler<TCommand, TResult> Decorated { get { return this.decorated; } }
	}
}