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
		public static IEnumerable<Type> GetAllDecoratorsTypes<TCommand> (this ICommandsProcessor commands)
			where TCommand : ICommand
		{
			return commands.GetAllDecorators<TCommand> ().Select (x => x.GetType ());
		}


		/// <summary>
		///     Returns the list of decorators generic types that registered for a given command.
		/// </summary>
		[DebuggerStepThrough, NotNull]
		public static IEnumerable<Type> GetAllDecoratorsGenericTypes<TCommand> (this ICommandsProcessor commands)
			where TCommand : ICommand
		{
			return commands.GetAllDecoratorsTypes<TCommand> ().Select (x => x.GetGenericTypeDefinition ());
		}


		/// <summary>
		///     Returns the list of decorators types that registered for a given command.
		/// </summary>
		[DebuggerStepThrough, NotNull]
		public static IEnumerable<Type> GetAllAsyncDecoratorsTypes<TCommand> (this ICommandsProcessor commands)
			where TCommand : IAsyncCommand
		{
			return commands.GetAllAsyncDecorators<TCommand> ().Select (x => x.GetType ());
		}


		/// <summary>
		///     Returns the list of decorators generic types that registered for a given command.
		/// </summary>
		[DebuggerStepThrough, NotNull]
		public static IEnumerable<Type> GetAllAsyncDecoratorsGenericTypes<TCommand> (this ICommandsProcessor commands)
			where TCommand : IAsyncCommand
		{
			return commands.GetAllAsyncDecoratorsTypes<TCommand> ().Select (x => x.GetGenericTypeDefinition ());
		}
	}
}