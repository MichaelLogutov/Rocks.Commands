using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Commands.Tests.AsyncCommands
{
	[UsedImplicitly]
	internal class TestCommandHandler : IAsyncCommandHandler<TestCommand, int>
	{
		public async Task<int> ExecuteAsync (TestCommand command, CancellationToken cancellationToken)
		{
			await Task.Yield ();

			command.Number++;

			return command.Number;
		}
	}
}