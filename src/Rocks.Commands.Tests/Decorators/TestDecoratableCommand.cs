namespace Rocks.Commands.Tests.Decorators
{
	public class TestDecoratableCommand : ICommand<int>, IDecoratableCommand
	{
		public int Number { get; set; }
	}
}