using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Commands.Async.Commands
{
	public class ResultAsyncCommand : IAsyncCommand<int>
	{
		public int Number { get; set; }
	}


	[UsedImplicitly]
	internal class ResultAsyncCommandHandler : IAsyncCommandHandler<ResultAsyncCommand, int>
	{
		public async Task<int> ExecuteAsync (ResultAsyncCommand command, CancellationToken cancellationToken = new CancellationToken ())
		{
			await Task.Yield ();

			command.Number++;
			return command.Number;
		}
	}
}