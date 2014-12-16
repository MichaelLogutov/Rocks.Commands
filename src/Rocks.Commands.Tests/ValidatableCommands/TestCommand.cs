using Rocks.Commands.Exceptions;

namespace Rocks.Commands.Tests.ValidatableCommands
{
	public class TestCommand : ICommand<Void>, IValidatableCommand
	{
		public int Number { get; set; }


		public void Validate ()
		{
			throw new CommandException ("Validated");
		}
	}
}