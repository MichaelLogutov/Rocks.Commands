using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocks.Commands.Extensions;
using Rocks.Commands.Tests.Decorators.Sync.Commands;

namespace Rocks.Commands.Tests.Decorators.Sync
{
	[TestClass]
	public class Tests
	{
		[TestMethod]
		public void RegisterCommandsDecorator_Always_RegistersAndUsesOpenGenericDecorators ()
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
		public void RegisterCommandsDecorator_DoesNotAppliesDecoratorToNotApplicableCommands ()
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


		[TestMethod]
		public void GetAllDecorators_NoDecorators_ReturnsNothing ()
		{
			// arrange
			CommandsLibrary.Setup ();


			// act
			var result = CommandsLibrary.CommandsProcessor.GetAllDecorators<TestDecoratableCommand, int> ();


			// assert
			result.Should ().BeEmpty ();
		}


		[TestMethod]
		public void GetAllDecoratorsGenericTypes_OneDecorator_ReturnsIt ()
		{
			// arrange
			CommandsLibrary.Setup ();
			CommandsLibrary.RegisterCommandsDecorator (typeof (TestDecorator<,>));


			// act
			var result = CommandsLibrary.CommandsProcessor.GetAllDecoratorsGenericTypes<TestDecoratableCommand, int> ();


			// assert
			result.Should ().Equal (typeof (TestDecorator<,>));
		}
	}
}