﻿namespace Rocks.Commands.Tests.LibraryA
{
	public class CrossLibraryTestCommand : ICommand<int>
	{
		public int Number { get; set; }
	}
}