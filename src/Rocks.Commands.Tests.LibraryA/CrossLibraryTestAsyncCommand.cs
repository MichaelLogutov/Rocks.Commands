namespace Rocks.Commands.Tests.LibraryA
{
	public class CrossLibraryTestAsyncCommand : IAsyncCommand<int>
	{
		public int Number { get; set; }
	}
}