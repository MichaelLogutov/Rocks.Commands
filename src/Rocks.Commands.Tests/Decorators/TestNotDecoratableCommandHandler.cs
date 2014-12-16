using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Decorators
{
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