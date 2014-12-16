using System;
using Rocks.Commands.Tests.LibraryA;

namespace Rocks.Commands.Tests.LibraryB
{
	public class CrossLibraryTestCommandHandler : ICommandHandler<CrossLibraryTestCommand, int>
	{
		public int Execute (CrossLibraryTestCommand command)
		{
			command.Number++;
			return command.Number;
		}
	}
}