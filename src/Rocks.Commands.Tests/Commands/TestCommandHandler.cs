using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Commands
{
	[UsedImplicitly]
	internal class TestCommandHandler : ICommandHandler<TestCommand, Void>, ICommandHandler<TestCommand2, Void>
	{
		public Void Execute (TestCommand command)
		{
			command.Number++;
			return Void.Result;
		}


		public Void Execute (TestCommand2 command)
		{
			command.Number++;
			return Void.Result;
		}
	}
}