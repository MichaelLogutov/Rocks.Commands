using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rocks.Commands
{
	/// <summary>
	///     A command processor.
	/// </summary>
	public interface ICommandsProcessor
	{
		/// <summary>
		///     Executes specified <paramref name="command" />.
		/// </summary>
		/// <typeparam name="TResult">Type of the command result.</typeparam>
		/// <param name="command">Command instance to execute.</param>
		[DebuggerStepThrough]
		TResult Execute<TResult> ([NotNull] ICommand<TResult> command);


		/// <summary>
		///     Executes specified <paramref name="command" /> asynchronously
		///     using <see cref="IAsyncCommandHandler{TCommand,TResult}"/> (needs to be registered).
		/// </summary>
		/// <typeparam name="TResult">Type of the command result.</typeparam>
		/// <param name="command">Command instance to execute.</param>
		[DebuggerStepThrough]
		Task<TResult> ExecuteAsync<TResult> ([NotNull] IAsyncCommand<TResult> command);


		/// <summary>
		///     Executes specified <paramref name="command" /> asynchronously
		///     using <see cref="IAsyncCommandHandler{TCommand,TResult}"/> (needs to be registered).
		/// </summary>
		/// <typeparam name="TResult">Type of the command result.</typeparam>
		/// <param name="command">Command instance to execute.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		[DebuggerStepThrough]
		Task<TResult> ExecuteAsync<TResult> ([NotNull] IAsyncCommand<TResult> command, CancellationToken cancellationToken);
	}
}