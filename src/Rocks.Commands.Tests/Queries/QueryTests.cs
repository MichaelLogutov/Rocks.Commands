using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rocks.Commands.Tests.Queries
{
	[TestClass]
	public class QueryTests
	{
		[TestMethod]
		public void ShouldRegisterAndHandleQuery ()
		{
			// arrange
			CommandsLibrary.Setup ();

			var query = new TestQuery { Number = 1 };


			// act
			var result = CommandsLibrary.CommandsProcessor.Execute (query);


			// assert
			result.Should ().Be (2);
		}
	}
}