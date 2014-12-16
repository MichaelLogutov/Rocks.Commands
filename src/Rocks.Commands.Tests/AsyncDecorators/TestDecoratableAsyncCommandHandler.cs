using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Commands.Tests.AsyncDecorators
{
	[UsedImplicitly]
	internal class TestDecoratableAsyncCommandHandler : IAsyncCommandHandler<TestDecoratableCommand, int>
	{
		public Task<int> ExecuteAsync (TestDecoratableCommand command, CancellationToken cancellationToken = new CancellationToken ())
		{
			command.Number++;
			return Task.FromResult (command.Number);
		}
	}
}