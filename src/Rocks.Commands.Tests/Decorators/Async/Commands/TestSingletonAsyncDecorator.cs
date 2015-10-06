using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rocks.Commands.Tests.Decorators.Async.Commands
{
    internal class TestSingletonAsyncDecorator<TCommand, TResult> : IAsyncCommandHandler<TCommand, TResult>,
                                                                    IDecorator
        where TCommand : IAsyncCommand<TResult>, IDecoratableAsyncCommand
    {
        private readonly Func<IAsyncCommandHandler<TCommand, TResult>> decorated;


        public TestSingletonAsyncDecorator (Func<IAsyncCommandHandler<TCommand, TResult>> decorated)
        {
            this.decorated = decorated;
        }


        public async Task<TResult> ExecuteAsync (TCommand command, CancellationToken cancellationToken = new CancellationToken ())
        {
            var result = await this.decorated ().ExecuteAsync (command, cancellationToken).ConfigureAwait (false);

            command.Number++;

            return result;
        }


        public object Decorated
        {
            get { return this.decorated; }
        }
    }
}