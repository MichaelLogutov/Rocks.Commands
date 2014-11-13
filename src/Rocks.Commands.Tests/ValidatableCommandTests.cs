using System;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocks.Commands.Exceptions;

namespace Rocks.Commands.Tests
{
	[TestClass]
	public class ValidatableCommandTests
	{
		public class TestCommand : ICommand<Void>, IValidatableCommand
		{
			public int Number { get; set; }


			public void Validate ()
			{
				throw new CommandException ("Validated");
			}
		}


		[UsedImplicitly]
		internal class TestCommandHandler : ICommandHandler<TestCommand, Void>
		{
			public Void Execute (TestCommand command)
			{
				command.Number++;
				return Void.Result;
			}
		}


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