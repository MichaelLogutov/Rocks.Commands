using System;
using FluentAssertions;
using Xunit;
using Rocks.Commands.Exceptions;
using Rocks.Commands.Tests.Commands.Sync.Commands;
using Rocks.Commands.Tests.LibraryA;
using Rocks.Commands.Tests.LibraryB;
using Rocks.Commands.Tests.LibraryC;
using Rocks.Commands.Tests.LibraryD;

namespace Rocks.Commands.Tests.Commands.Sync
{
	public class Tests
	{
		[Fact]
		public void Execute_ResultCommand_Executes ()
		{
			// arrange
			CommandsLibrary.Setup ();

			var command = new ResultCommand { Number = 1 };


			// act
			var result = CommandsLibrary.CommandsProcessor.Execute (command);


			// assert
			result.Should ().Be (2);
			command.Number.Should ().Be (2);
		}


		[Fact]
		public void Execute_ArbitraryCommand_Always_Executes ()
		{
			// arrange
			CommandsLibrary.Setup ();

			var command = new ResultCommand { Number = 1 };


			// act
			var result = CommandsLibrary.CommandsProcessor.Execute ((ICommand) command);


			// assert
			result.Should ().Be (2);
			command.Number.Should ().Be (2);
		}


		[Fact]
		public void Execute_CommandAndHandlerInDifferentLibraries_Executes ()
		{
			// arrange
			CommandsLibrary.Setup (assemblies: new[] { typeof (ILibraryA).Assembly, typeof (ILibraryB).Assembly });

			var command = new CrossLibraryTestCommand { Number = 1 };


			// act
			var result = CommandsLibrary.CommandsProcessor.Execute (command);


			// assert
			result.Should ().Be (2);
			command.Number.Should ().Be (2);
		}


		[Fact]
		public void Setup_CommandWithoutHandler_Throws ()
		{
			// arrange


			// act
			var action = new Action (() => CommandsLibrary.Setup (assemblies: new[] { typeof (ILibraryC).Assembly }));


			// assert
			action.ShouldThrow<CommandHandlerNotFoundException> ()
			      .Which
			      .ShouldBeEquivalentTo (new CommandHandlerNotFoundException
			                             {
				                             CommandType = typeof (TestCommandWithoutHandler),
				                             ResultType = typeof (Void)
			                             },
			                             options => options.Including (x => x.CommandType)
			                                               .Including (x => x.ResultType));
		}


		[Fact]
		public void Setup_CommandWithPartialHandler_Throws ()
		{
			// arrange


			// act
			var action = new Action (() => CommandsLibrary.Setup (assemblies: new[] { typeof (ILibraryD).Assembly }));


			// assert
			action.ShouldThrow<CommandHandlerNotFoundException> ()
			      .Which
			      .ShouldBeEquivalentTo (new CommandHandlerNotFoundException
			                             {
				                             CommandType = typeof (TestCommandWithPartialHandler),
				                             ResultType = typeof (int)
			                             },
			                             options => options.Including (x => x.CommandType)
			                                               .Including (x => x.ResultType));
		}


		[Fact]
		public void ValidatableCommand_CallsValidateBeforeExecuting ()
		{
			// arrange
			CommandsLibrary.Setup ();

			var query = new ValidatableCommand { Number = 1 };


			// act
			var action = new Action (() => CommandsLibrary.CommandsProcessor.Execute (query));


			// assert
			action.ShouldThrow<TestException> ();
			query.Number.Should ().Be (1);
		}
	}
}


