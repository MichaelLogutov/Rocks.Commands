using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Decorators
{
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