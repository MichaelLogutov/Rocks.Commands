using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Rocks.Commands.Exceptions;
using Rocks.Commands.Tests.Commands.Async.Commands;
using Rocks.Commands.Tests.LibraryA;
using Rocks.Commands.Tests.LibraryB;
using Rocks.Commands.Tests.LibraryE;
using Rocks.Commands.Tests.LibraryF;

namespace Rocks.Commands.Tests.Commands.Async
{
	public class Tests
	{
		[Fact]
		public async Task ExecuteAsync_ResultCommand_Executes ()
		{
			// arrange
			CommandsLibrary.Setup ();

			var command = new ResultAsyncCommand { Number = 1 };


			// act
			var result = await CommandsLibrary.CommandsProcessor.ExecuteAsync (command);


			// assert
			result.Should ().Be (2);
			command.Number.Should ().Be (2);
		}


		[Fact]
		public async Task ExecuteAsync_ArbitraryCommand_Always_Executes ()
		{
			// arrange
			CommandsLibrary.Setup ();

			var command = new ResultAsyncCommand { Number = 1 };


			// act
			var result = await CommandsLibrary.CommandsProcessor.ExecuteAsync ((IAsyncCommand) command);


			// assert
			result.Should ().Be (2);
			command.Number.Should ().Be (2);
		}


		[Fact]
		public async Task ExecuteAsync_CommandAndHandlerInDifferentLibraries_Executes ()
		{
			// arrange
			CommandsLibrary.Setup (assemblies: new[] { typeof (ILibraryA).Assembly, typeof (ILibraryB).Assembly });

			var command = new CrossLibraryTestAsyncCommand { Number = 1 };


			// act
			var result = await CommandsLibrary.CommandsProcessor.ExecuteAsync (command);


			// assert
			result.Should ().Be (2);
			command.Number.Should ().Be (2);
		}


		[Fact]
		public void Setup_CommandWithoutHandler_Throws ()
		{
			// arrange


			// act
			var action = new Action (() => CommandsLibrary.Setup (assemblies: new[] { typeof (ILibraryF).Assembly }));


			// assert
			action.Should().Throw<CommandHandlerNotFoundException> ()
			      .Which
			      .Should().BeEquivalentTo (new CommandHandlerNotFoundException
			                             {
				                             CommandType = typeof (TestAsyncCommandWithoutHandler),
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
			var action = new Action (() => CommandsLibrary.Setup (assemblies: new[] { typeof (ILibraryE).Assembly }));


			// assert
			action.Should().Throw<CommandHandlerNotFoundException> ()
			      .Which
			      .Should().BeEquivalentTo (new CommandHandlerNotFoundException
			                             {
				                             CommandType = typeof (TestAsyncCommandWithPartialHandler),
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

			var query = new ValidatableAsyncCommand { Number = 1 };


			// act
			var action = new Action (() => CommandsLibrary.CommandsProcessor.ExecuteAsync (query).Wait ());


			// assert
			action.Should().Throw<TestException> ();
			query.Number.Should ().Be (1);
		}
	}
}


