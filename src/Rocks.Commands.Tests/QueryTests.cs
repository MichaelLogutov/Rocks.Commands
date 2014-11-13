using System;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Commands.Tests
{
	[TestClass]
	public class QueryTests
	{
		public class TestQuery : ICommand<int>
		{
			public int Number { get; set; }
		}


		[UsedImplicitly]
		internal class TestQueryHandler : ICommandHandler<TestQuery, int>
		{
			public int Execute (TestQuery query)
			{
				return query.Number + 1;
			}
		}


		[TestMethod]
		public void ShouldRegisterAndHandleQuery ()
		{
			// arrange
			Commands.Setup ();

			var query = new TestQuery { Number = 1 };


			// act
			var result = Commands.CommandsProcessor.Execute (query);


			// assert
			result.Should ().Be (2);
		}
	}
}