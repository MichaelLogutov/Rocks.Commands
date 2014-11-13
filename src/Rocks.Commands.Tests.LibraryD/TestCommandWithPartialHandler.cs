namespace Rocks.Commands.Tests.LibraryD
{
	public class TestCommandWithPartialHandler : ICommand<Void>, ICommand<int>
	{
	}


	internal class TestCommandWithPartialHandlerCommandHandler : ICommandHandler<TestCommandWithPartialHandler, Void>
	{
		public Void Execute (TestCommandWithPartialHandler command)
		{
			return Void.Result;
		}
	}
}