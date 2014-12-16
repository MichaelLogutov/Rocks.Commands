using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Commands.Sync.Commands
{
	public class VoidCommand : ICommand<Void>
	{
		public int Number { get; set; }
	}


	public class VoidCommand2 : ICommand<Void>
	{
		public int Number { get; set; }
	}


	[UsedImplicitly]
	internal class VoidCommandHandler : ICommandHandler<VoidCommand, Void>, ICommandHandler<VoidCommand2, Void>
	{
		public Void Execute (VoidCommand command)
		{
			command.Number++;
			return Void.Result;
		}


		public Void Execute (VoidCommand2 command)
		{
			command.Number++;
			return Void.Result;
		}
	}
}