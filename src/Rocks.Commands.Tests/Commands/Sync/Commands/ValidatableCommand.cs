using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Commands.Sync.Commands
{
	public class ValidatableCommand : ICommand<Void>, IValidatableCommand
	{
		public int Number { get; set; }


		public void Validate ()
		{
			throw new TestException ();
		}
	}


	[UsedImplicitly]
	internal class ValidatableCommandHandler : ICommandHandler<ValidatableCommand, Void>
	{
		public Void Execute (ValidatableCommand command)
		{
			command.Number++;
			return Void.Result;
		}
	}
}