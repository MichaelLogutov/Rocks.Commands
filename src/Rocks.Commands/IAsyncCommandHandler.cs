using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Commands
{
	/// <summary>
	///     An asynchronous command handler.
	/// </summary>
	/// <typeparam name="TCommand">Type of the command to handle.</typeparam>
	/// <typeparam name="TResult">Type of the command result.</typeparam>
	public interface IAsyncCommandHandler<in TCommand, TResult>
		where TCommand : IAsyncCommand<TResult>
	{
		/// <summary>
		///     Handles <paramref name="command" /> execution.
		/// </summary>
		/// <param name="command">Command to be executed.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		Task<TResult> ExecuteAsync ([NotNull] TCommand command, CancellationToken cancellationToken);
	}
}