namespace Rocks.Commands.Tests.Decorators.Sync.Commands
{
	internal class TestDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>,
	                                                  IDecorator
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


		public object Decorated { get { return this.decorated; } }
	}
}