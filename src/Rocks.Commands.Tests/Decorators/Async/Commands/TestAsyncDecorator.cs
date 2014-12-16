using System.Threading;
using System.Threading.Tasks;

namespace Rocks.Commands.Tests.Decorators.Async.Commands
{
	internal class TestAsyncDecorator<TCommand, TResult> : IAsyncCommandHandler<TCommand, TResult>,
	                                                       IAsyncDecorator<TCommand, TResult>
		where TCommand : IAsyncCommand<TResult>, IDecoratableAsyncCommand
	{
		private readonly IAsyncCommandHandler<TCommand, TResult> decorated;


		public TestAsyncDecorator (IAsyncCommandHandler<TCommand, TResult> decorated)
		{
			this.decorated = decorated;
		}


		public async Task<TResult> ExecuteAsync (TCommand command, CancellationToken cancellationToken = new CancellationToken ())
		{
			var result = await this.decorated.ExecuteAsync (command, cancellationToken).ConfigureAwait (false);

			command.Number++;

			return result;
		}


		IAsyncCommandHandler<TCommand, TResult> IAsyncDecorator<TCommand, TResult>.Decorated { get { return this.decorated; } }
	}
}