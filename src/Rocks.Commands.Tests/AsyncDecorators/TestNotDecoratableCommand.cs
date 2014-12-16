namespace Rocks.Commands.Tests.AsyncDecorators
{
	public class TestNotDecoratableCommand : IAsyncCommand<int>
	{
		public int Number { get; set; }
	}
}