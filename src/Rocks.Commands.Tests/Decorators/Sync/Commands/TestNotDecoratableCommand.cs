using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Decorators.Sync.Commands
{
	public class TestNotDecoratableCommand : ICommand<int>
	{
		public int Number { get; set; }
	}


	[UsedImplicitly]
	internal class TestNotDecoratableCommandHandler : ICommandHandler<TestNotDecoratableCommand, int>
	{
		public int Execute (TestNotDecoratableCommand command)
		{
			command.Number++;
			return command.Number;
		}
	}
}