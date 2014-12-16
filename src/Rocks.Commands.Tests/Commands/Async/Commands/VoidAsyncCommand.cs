using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Commands.Async.Commands
{
	public class VoidAsyncCommand : IAsyncCommand<Void>
	{
		public int Number { get; set; }
	}


	public class VoidAsyncCommand2 : IAsyncCommand<Void>
	{
		public int Number { get; set; }
	}


	[UsedImplicitly]
	internal class VoidAsyncCommandHandler : IAsyncCommandHandler<VoidAsyncCommand, Void>, IAsyncCommandHandler<VoidAsyncCommand2, Void>
	{
		public async Task<Void> ExecuteAsync (VoidAsyncCommand command, CancellationToken cancellationToken = new CancellationToken ())
		{
			await Task.Yield ();

			command.Number++;
			return Void.Result;
		}


		public async Task<Void> ExecuteAsync (VoidAsyncCommand2 command, CancellationToken cancellationToken = new CancellationToken ())
		{
			await Task.Yield ();

			command.Number++;
			return Void.Result;
		}
	}
}