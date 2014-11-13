using System;
using JetBrains.Annotations;

namespace Rocks.Commands
{
	/// <summary>
	///     A factory for every command handler.
	/// </summary>
	public interface ICommandHandlerFactory
	{
		/// <summary>
		///     Retrieve a command handler object instance with specified <paramref name="commandHandlerType" />.
		/// </summary>
		/// <param name="commandHandlerType">Type of the command handler that should be retrieved.</param>
		[NotNull]
		object GetCommandHandler ([NotNull] Type commandHandlerType);
	}
}