using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Commands.Tests
{
	[TestClass]
	public class IAsyncCommandTests
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
				return await Task.Run (() =>
				{
					command.Number++;
					return command.Number;
				}, cancellationToken);
			}
		}


		[TestMethod]
		public void ShouldRegisterAndHandleCommand ()
		{
			// arrange
			Commands.Setup ();

			var command = new TestCommand { Number = 1 };


			// act
			var result = Commands.CommandsProcessor.ExecuteAsync (command).Result;


			// assert
			result.Should ().Be (2);
			command.Number.Should ().Be (2);
		}
	}
}
