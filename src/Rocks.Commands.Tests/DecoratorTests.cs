using System;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Commands.Tests
{
	[TestClass]
	public class DecoratorTests
	{
		internal interface IDecoratableCommand
		{
			int Number { get; set; }
		}


		public class TestDecoratableCommand : ICommand<int>, IDecoratableCommand
		{
			public int Number { get; set; }
		}


		[UsedImplicitly]
		internal class TestDecoratableCommandHandler : ICommandHandler<TestDecoratableCommand, int>
		{
			public int Execute (TestDecoratableCommand command)
			{
				command.Number++;
				return command.Number;
			}
		}


		public class TestNotDecoratableCommand : ICommand<int>
		{
			public int Number { get; set; }
		}


		[UsedImplicitly]
		internal class TestNotDecoratableCommandHandler : ICommandHandler<TestNotDecoratableCommand, int>
		{
			public int Execute (TestNotDecoratableCommand command)
			{
				command.Number++;
				return command.Number;
			}
		}


		internal class TestDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
			where TCommand : ICommand<TResult>, IDecoratableCommand
		{
			private readonly ICommandHandler<TCommand, TResult> decorated;


			public TestDecorator (ICommandHandler<TCommand, TResult> decorated)
			{
				this.decorated = decorated;
			}


			public TResult Execute (TCommand command)
			{
				var result = this.decorated.Execute (command);
				command.Number++;
				return result;
			}
		}


		[TestMethod]
		public void ShouldRegisterAndUseOpenGenericDecorator ()
		{
			// arrange
			CommandsLibrary.Setup ();
			CommandsLibrary.RegisterCommandsDecorator (typeof (TestDecorator<,>));

			var command = new TestDecoratableCommand { Number = 1 };


			// act
			var result = CommandsLibrary.CommandsProcessor.Execute (command);


			// assert
			command.Number.Should ().Be (3);
			result.Should ().Be (2);
		}


		[TestMethod]
		public void OpenGenericDecoratorShouldNotBeAppliedToNotSupportedCommands ()
		{
			// arrange
			CommandsLibrary.Setup ();
			CommandsLibrary.RegisterCommandsDecorator (typeof (TestDecorator<,>));

			var command = new TestNotDecoratableCommand { Number = 1 };


			// act
			var result = CommandsLibrary.CommandsProcessor.Execute (command);


			// assert
			command.Number.Should ().Be (2);
			result.Should ().Be (2);
		}
	}
}