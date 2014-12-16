namespace Rocks.Commands.Tests.AsyncDecorators
{
	public class TestDecoratableCommand : IAsyncCommand<int>, IDecoratableCommand
	{
		public int Number { get; set; }
	}
}