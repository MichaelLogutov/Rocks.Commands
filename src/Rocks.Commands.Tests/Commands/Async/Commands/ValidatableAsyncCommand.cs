using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Commands.Async.Commands
{
	public class ValidatableAsyncCommand : IAsyncCommand<Void>, IValidatableCommand
	{
		public int Number { get; set; }


		public void Validate ()
		{
			throw new TestException ();
		}
	}


	[UsedImplicitly]
	internal class ValidatableAsyncCommandHandler : IAsyncCommandHandler<ValidatableAsyncCommand, Void>
	{
		public async Task<Void> ExecuteAsync (ValidatableAsyncCommand command, CancellationToken cancellationToken = new CancellationToken ())
		{
			await Task.Yield ();

			command.Number++;
			return Void.Result;
		}
	}
}