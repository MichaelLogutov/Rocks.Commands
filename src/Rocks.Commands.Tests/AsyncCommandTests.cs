using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Commands.Tests
{
	[TestClass]
	public class AsyncCommandTests
	{
		public class TestCommand : IAsyncCommand<int>
		{
			public int Number { get; set; }
		}


		[UsedImplicitly]
		internal class TestCommandHandler : IAsyncCommandHandler<TestCommand, int>
		{
			public async Task<int> ExecuteAsync (TestCommand command, CancellationToken cancellationToken)
			{
				await Task.Yield ();

				command.Number++;

				return command.Number;
			}
		}


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