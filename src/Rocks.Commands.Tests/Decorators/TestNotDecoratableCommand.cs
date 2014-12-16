namespace Rocks.Commands.Tests.Decorators
{
	public class TestNotDecoratableCommand : ICommand<int>
	{
		public int Number { get; set; }
	}
}