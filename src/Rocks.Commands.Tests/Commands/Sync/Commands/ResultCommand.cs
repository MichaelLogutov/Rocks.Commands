using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Commands.Sync.Commands
{
	public class ResultCommand : ICommand<int>
	{
		public int Number { get; set; }
	}


	[UsedImplicitly]
	internal class ResultCommandHandler : ICommandHandler<ResultCommand, int>
	{
		public int Execute (ResultCommand command)
		{
			command.Number++;
			return command.Number;
		}
	}
}