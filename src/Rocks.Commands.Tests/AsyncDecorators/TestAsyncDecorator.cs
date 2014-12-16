using System.Threading;
using System.Threading.Tasks;

namespace Rocks.Commands.Tests.AsyncDecorators
{
	internal class TestAsyncDecorator<TCommand, TResult> : IAsyncCommandHandler<TCommand, TResult>,
	                                                       IAsyncDecorator<TCommand, TResult>
		where TCommand : IAsyncCommand<TResult>, IDecoratableCommand
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


		/// <summary>
		///     A decorated command handler.
		/// </summary>
		IAsyncCommandHandler<TCommand, TResult> IAsyncDecorator<TCommand, TResult>.Decorated { get { return this.decorated; } }
	}
}