using System;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocks.Commands.Extensions;

namespace Rocks.Commands.Tests.AsyncDecorators
{
	[TestClass]
	public class AsyncDecoratorTests
	{
		[TestMethod]
		public async Task RegisterCommandsDecorator_Always_RegistersAndUsesOpenGenericDecorators ()
		{
			// arrange
			CommandsLibrary.Setup ();
			CommandsLibrary.RegisterCommandsDecorator (typeof (TestAsyncDecorator<,>));

			var command = new TestDecoratableCommand { Number = 1 };


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
			CommandsLibrary.RegisterCommandsDecorator (typeof (TestAsyncDecorator<,>));

			var command = new TestNotDecoratableCommand { Number = 1 };


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
			var result = CommandsLibrary.CommandsProcessor.GetAllAsyncDecorators<TestDecoratableCommand, int> ();


			// assert
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void GetAllDecoratorsGenericTypes_OneDecorator_ReturnsIt ()
		{
			// arrange
			CommandsLibrary.Setup ();
			CommandsLibrary.RegisterCommandsDecorator (typeof (TestAsyncDecorator<,>));


			// act
			var result = CommandsLibrary.CommandsProcessor.GetAllAsyncDecoratorsGenericTypes<TestDecoratableCommand, int> ();


			// assert
			result.Should ().Equal (typeof (TestAsyncDecorator<,>));
		}
	}
}