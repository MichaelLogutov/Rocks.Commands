using JetBrains.Annotations;

namespace Rocks.Commands.Tests.ValidatableCommands
{
	[UsedImplicitly]
	internal class TestCommandHandler : ICommandHandler<TestCommand, Void>
	{
		public Void Execute (TestCommand command)
		{
			command.Number++;
			return Void.Result;
		}
	}
}