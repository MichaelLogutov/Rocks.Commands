using System;
using Rocks.Commands.Tests.LibraryA;

namespace Rocks.Commands.Tests.LibraryB
{
	public class CrossLibraryTestCommandHandler : ICommandHandler<CrossLibraryTestCommand, Void>
	{
		public Void Execute (CrossLibraryTestCommand command)
		{
			command.Number++;
			return Void.Result;
		}
	}
}