namespace Rocks.Commands.Tests.LibraryA
{
	public class CrossLibraryTestCommand : ICommand<Void>
	{
		public int Number { get; set; }
	}
}