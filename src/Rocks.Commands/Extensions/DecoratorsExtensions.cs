using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Rocks.Commands.Extensions
{
	public static class DecoratorsExtensions
	{
		/// <summary>
		///     Returns the list of decorators types that registered for a given command.
		/// </summary>
		[DebuggerStepThrough, NotNull]
		public static IEnumerable<Type> GetAllDecoratorsTypes<TCommand, TResult> (this ICommandsProcessor commands)
			where TCommand : ICommand<TResult>
		{
			return commands.GetAllDecorators<TCommand, TResult> ().Select (x => x.GetType ());
		}


		/// <summary>
		///     Returns the list of decorators generic types that registered for a given command.
		/// </summary>
		[DebuggerStepThrough, NotNull]
		public static IEnumerable<Type> GetAllDecoratorsGenericTypes<TCommand, TResult> (this ICommandsProcessor commands)
			where TCommand : ICommand<TResult>
		{
			return commands.GetAllDecoratorsTypes<TCommand, TResult> ().Select (x => x.GetGenericTypeDefinition ());
		}


		/// <summary>
		///     Returns the list of decorators types that registered for a given command.
		/// </summary>
		[DebuggerStepThrough, NotNull]
		public static IEnumerable<Type> GetAllAsyncDecoratorsTypes<TCommand, TResult> (this ICommandsProcessor commands)
			where TCommand : IAsyncCommand<TResult>
		{
			return commands.GetAllAsyncDecorators<TCommand, TResult> ().Select (x => x.GetType ());
		}


		/// <summary>
		///     Returns the list of decorators generic types that registered for a given command.
		/// </summary>
		[DebuggerStepThrough, NotNull]
		public static IEnumerable<Type> GetAllAsyncDecoratorsGenericTypes<TCommand, TResult> (this ICommandsProcessor commands)
			where TCommand : IAsyncCommand<TResult>
		{
			return commands.GetAllAsyncDecoratorsTypes<TCommand, TResult> ().Select (x => x.GetGenericTypeDefinition ());
		}
	}
}