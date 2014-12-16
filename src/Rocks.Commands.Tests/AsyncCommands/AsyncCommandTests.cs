using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Commands.Tests.AsyncCommands
{
	[TestClass]
	public class AsyncCommandTests
	{
		[TestMethod]
		public async Task Always_RegisterAndHandlesTheCommand ()
		{
			// arrange
			CommandsLibrary.Setup ();

			var command = new TestCommand { Number = 1 };


			// act
			var result = await CommandsLibrary.CommandsProcessor.ExecuteAsync (command);


			// assert
			result.Should ().Be (2);
			command.Number.Should ().Be (2);
		}


		[TestMethod]
		public async Task ExecuteArbitraryCommand_HandlesTheCommand ()
		{
			// arrange
			CommandsLibrary.Setup ();

			var command = new TestCommand { Number = 1 };


			// act
			var result = await CommandsLibrary.CommandsProcessor.ExecuteAsync ((IAsyncCommand) command);


			// assert
			result.Should ().Be (2);
			command.Number.Should ().Be (2);
		}
	}
}