namespace Rocks.Commands.Tests.LibraryE
{
	public class TestAsyncCommandWithPartialHandler : ICommand<Void>, ICommand<int>
	{
	}


	internal class TestAsyncCommandWithPartialHandlerCommandHandler : ICommandHandler<TestAsyncCommandWithPartialHandler, Void>
	{
		public Void Execute (TestAsyncCommandWithPartialHandler command)
		{
			return Void.Result;
		}
	}
}