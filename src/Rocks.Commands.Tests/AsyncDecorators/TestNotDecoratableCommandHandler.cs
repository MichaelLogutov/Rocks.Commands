using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Commands.Tests.AsyncDecorators
{
	[UsedImplicitly]
	internal class TestNotDecoratableCommandHandler : IAsyncCommandHandler<TestNotDecoratableCommand, int>
	{
		public Task<int> ExecuteAsync (TestNotDecoratableCommand command, CancellationToken cancellationToken = new CancellationToken ())
		{
			command.Number++;
			return Task.FromResult (command.Number);
		}
	}
}