using System;

namespace Rocks.Commands.Tests.Decorators.Sync.Commands
{
    internal class TestSingletonDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>,
                                                               IDecorator
        where TCommand : ICommand<TResult>, IDecoratableCommand
    {
        private readonly Func<ICommandHandler<TCommand, TResult>> decorated;


        public TestSingletonDecorator (Func<ICommandHandler<TCommand, TResult>> decorated)
        {
            this.decorated = decorated;
        }


        public TResult Execute (TCommand command)
        {
            var result = this.decorated ().Execute (command);
            command.Number++;
            return result;
        }


        public object Decorated
        {
            get { return this.decorated; }
        }
    }
}