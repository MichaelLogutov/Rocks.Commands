using System;
using System.Threading;
using System.Threading.Tasks;
using Rocks.Commands.Tests.LibraryA;

namespace Rocks.Commands.Tests.LibraryB
{
	public class CrossLibraryTestCommandAsyncHandler : IAsyncCommandHandler<CrossLibraryTestAsyncCommand, int>
	{
		public async Task<int> ExecuteAsync (CrossLibraryTestAsyncCommand command, CancellationToken cancellationToken = new CancellationToken ())
		{
			await Task.Yield ();

			command.Number++;
			return command.Number;
		}
	}
}