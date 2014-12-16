using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Decorators.Sync.Commands
{
	public class TestDecoratableCommand : ICommand<int>, IDecoratableCommand
	{
		public int Number { get; set; }
	}


	[UsedImplicitly]
	internal class TestDecoratableCommandHandler : ICommandHandler<TestDecoratableCommand, int>
	{
		public int Execute (TestDecoratableCommand command)
		{
			command.Number++;
			return command.Number;
		}
	}
}