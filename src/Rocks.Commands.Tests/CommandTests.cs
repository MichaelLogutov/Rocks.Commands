using System;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rocks.Commands.Exceptions;
using Rocks.Commands.Tests.LibraryA;
using Rocks.Commands.Tests.LibraryB;
using Rocks.Commands.Tests.LibraryC;
using Rocks.Commands.Tests.LibraryD;

namespace Rocks.Commands.Tests
{
	[TestClass]
	public class CommandTests
	{
		#region Public methods

		[TestMethod]
		public void Execute_CommandWithVoid_Executes ()
		{
			// arrange
			CommandsLibrary.Setup ();

			var command = new TestCommand { Number = 1 };


			// act
			var result = CommandsLibrary.CommandsProcessor.Execute (command);


			// assert
			result.Should ().Be (Void.Result);
			command.Number.Should ().Be (2);
		}


		[TestMethod]
		public void Execute_CommandAndHandlerInDifferentLibraries_Executes ()
		{
			// arrange
			CommandsLibrary.Setup (assemblies: new[] { typeof (ILibraryA).Assembly, typeof (ILibraryB).Assembly });

			var command = new CrossLibraryTestCommand { Number = 1 };


			// act
			var result = CommandsLibrary.CommandsProcessor.Execute (command);


			// assert
			result.Should ().Be (Void.Result);
			command.Number.Should ().Be (2);
		}



		[TestMethod]
		public void Setup_CommandHandlerWithTwoCommands_NotThrows ()
		{
			// arrange


			// act
			var action = new Action (() => CommandsLibrary.Setup ());


			// assert
			action.ShouldNotThrow ();
		}


		[TestMethod]
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


		[TestMethod]
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


		[TestMethod]
		public void ExecuteArbitraryCommand_HandlesTheCommand ()
		{
			// arrange
			CommandsLibrary.Setup ();

			var command = new TestCommand { Number = 1 };


			// act
			var result = CommandsLibrary.CommandsProcessor.Execute ((ICommand) command);


			// assert
			result.Should ().Be (Void.Result);
			command.Number.Should ().Be (2);
		}

		#endregion

		#region Nested type: TestCommand

		public class TestCommand : ICommand<Void>
		{
			public int Number { get; set; }
		}

		#endregion

		#region Nested type: TestCommand2

		public class TestCommand2 : ICommand<Void>
		{
			public int Number { get; set; }
		}

		#endregion

		#region Nested type: TestCommandHandler

		[UsedImplicitly]
		internal class TestCommandHandler : ICommandHandler<TestCommand, Void>, ICommandHandler<TestCommand2, Void>
		{
			public Void Execute (TestCommand command)
			{
				command.Number++;
				return Void.Result;
			}


			public Void Execute (TestCommand2 command)
			{
				command.Number++;
				return Void.Result;
			}
		}

		#endregion
	}
}