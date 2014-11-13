using JetBrains.Annotations;

namespace Rocks.Commands
{
	/// <summary>
	///     A command handler.
	/// </summary>
	/// <typeparam name="TCommand">Type of the command to handle.</typeparam>
	/// <typeparam name="TResult">Type of the command result.</typeparam>
	public interface ICommandHandler<in TCommand, out TResult>
		where TCommand : ICommand<TResult>
	{
		/// <summary>
		///     Handles <paramref name="command" /> execution.
		/// </summary>
		/// <param name="command">Command to be executed.</param>
		TResult Execute ([NotNull] TCommand command);
	}
}