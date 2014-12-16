using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Decorators.Async.Commands
{
	public class TestNotDecoratableAsyncCommand : IAsyncCommand<int>
	{
		public int Number { get; set; }
	}


	[UsedImplicitly]
	internal class TestNotDecoratableAsyncCommandHandler : IAsyncCommandHandler<TestNotDecoratableAsyncCommand, int>
	{
		public Task<int> ExecuteAsync (TestNotDecoratableAsyncCommand command, CancellationToken cancellationToken = new CancellationToken ())
		{
			command.Number++;
			return Task.FromResult (command.Number);
		}
	}
}