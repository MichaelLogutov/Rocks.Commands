using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Decorators.Async.Commands
{
	public class TestDecoratableAsyncCommand : IAsyncCommand<int>, IDecoratableAsyncCommand
	{
		public int Number { get; set; }
	}


	[UsedImplicitly]
	internal class TestDecoratableAsyncCommandHandler : IAsyncCommandHandler<TestDecoratableAsyncCommand, int>
	{
		public Task<int> ExecuteAsync (TestDecoratableAsyncCommand command, CancellationToken cancellationToken = new CancellationToken ())
		{
			command.Number++;
			return Task.FromResult (command.Number);
		}
	}
}