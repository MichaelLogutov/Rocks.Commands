﻿using System.Collections.Generic;
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
		///     Executes arbitrary <paramref name="command" />.
		/// </summary>
		/// <param name="command">Command instance to execute.</param>
		[DebuggerStepThrough]
		object Execute ([NotNull] ICommand command);


		/// <summary>
		///     Executes specified <paramref name="command" /> asynchronously
		///     using <see cref="IAsyncCommandHandler{TCommand,TResult}"/> (needs to be registered).
		/// </summary>
		/// <typeparam name="TResult">Type of the command result.</typeparam>
		/// <param name="command">Command instance to execute.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		[DebuggerStepThrough]
		Task<TResult> ExecuteAsync<TResult> ([NotNull] IAsyncCommand<TResult> command, CancellationToken cancellationToken = default (CancellationToken));


		/// <summary>
		///     Executes arbitrary <paramref name="command" />.
		/// </summary>
		/// <param name="command">Command instance to execute.</param>
		/// <param name="cancellationToken">Task cancellation token.</param>
		[DebuggerStepThrough]
		Task<object> ExecuteAsync ([NotNull] IAsyncCommand command, CancellationToken cancellationToken = default (CancellationToken));


		/// <summary>
		///     Returns the list of <see cref="IDecorator" /> decorators instances
		///     that registered for a given command.
		/// </summary>
		[DebuggerStepThrough, NotNull]
		IList<IDecorator> GetAllDecorators<TCommand> () where TCommand : ICommand;


		/// <summary>
		///     Returns the list of <see cref="IDecorator" /> decorators instances
		///     that registered for a given command.
		/// </summary>
		[DebuggerStepThrough, NotNull]
		IList<IDecorator> GetAllAsyncDecorators<TCommand> () where TCommand : IAsyncCommand;
	}
}