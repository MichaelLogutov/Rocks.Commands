using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocks.Commands.Exceptions;

namespace Rocks.Commands.Tests.ValidatableCommands
{
	[TestClass]
	public class ValidatableCommandTests
	{
		[TestMethod]
		public void ShouldVerifyCommandBeforeHandling ()
		{
			// arrange
			CommandsLibrary.Setup ();

			var query = new TestCommand { Number = 1 };


			// act
			var action = new Action (() => CommandsLibrary.CommandsProcessor.Execute (query));


			// assert
			action.ShouldThrow<CommandException> ().WithMessage ("Validated");
			query.Number.Should ().Be (1);
		}
	}
}