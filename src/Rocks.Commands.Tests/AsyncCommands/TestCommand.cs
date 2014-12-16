namespace Rocks.Commands.Tests.AsyncCommands
{
	public class TestCommand : IAsyncCommand<int>
	{
		public int Number { get; set; }
	}
}