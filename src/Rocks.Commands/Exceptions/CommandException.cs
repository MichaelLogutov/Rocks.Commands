using System;
using JetBrains.Annotations;

namespace Rocks.Commands.Exceptions
{
	/// <summary>
	///     An exception that typically thrown from command handlers.
	/// </summary>
	[Serializable, UsedImplicitly]
	public class CommandException : InvalidOperationException
	{
		public CommandException (string message, params object[] args) : base (string.Format (message, args))
		{
		}
	}
}