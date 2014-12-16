using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocks.Commands.Extensions;
using Rocks.Commands.Tests.Decorators.Async.Commands;

namespace Rocks.Commands.Tests.Decorators.Async
{
	[TestClass]
	public class Tests
	{
		[TestMethod]
		public async Task RegisterCommandsDecorator_Always_RegistersAndUsesOpenGenericDecorators ()
		{
			// arrange
			CommandsLibrary.Setup ();
			CommandsLibrary.RegisterAsyncCommandsDecorator (typeof (TestAsyncDecorator<,>));

			var command = new TestDecoratableAsyncCommand { Number = 1 };


			// act
			var result = await CommandsLibrary.CommandsProcessor.ExecuteAsync (command);


			// assert
			command.Number.Should ().Be (3);
			result.Should ().Be (2);
		}


		[TestMethod]
		public async Task RegisterCommandsDecorator_DoesNotAppliesDecoratorToNotApplicableCommands ()
		{
			// arrange
			CommandsLibrary.Setup ();
			CommandsLibrary.RegisterAsyncCommandsDecorator (typeof (TestAsyncDecorator<,>));

			var command = new TestNotDecoratableAsyncCommand { Number = 1 };


			// act
			var result = await CommandsLibrary.CommandsProcessor.ExecuteAsync (command);


			// assert
			command.Number.Should ().Be (2);
			result.Should ().Be (2);
		}


		[TestMethod]
		public void GetAllDecorators_NoDecorators_ReturnsNothing ()
		{
			// arrange
			CommandsLibrary.Setup ();


			// act
			var result = CommandsLibrary.CommandsProcessor.GetAllAsyncDecorators<TestDecoratableAsyncCommand, int> ();


			// assert
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void GetAllDecoratorsGenericTypes_OneDecorator_ReturnsIt ()
		{
			// arrange
			CommandsLibrary.Setup ();
			CommandsLibrary.RegisterAsyncCommandsDecorator (typeof (TestAsyncDecorator<,>));


			// act
			var result = CommandsLibrary.CommandsProcessor.GetAllAsyncDecoratorsGenericTypes<TestDecoratableAsyncCommand, int> ();


			// assert
			result.Should ().Equal (typeof (TestAsyncDecorator<,>));
		}
	}
}