using System;

namespace Rocks.Commands.Exceptions
{
	/// <summary>
	///     Exception thrown by
	///     <see cref="CommandsLibrary.Setup" />
	///     or <see cref="CommandsLibrary.RegisterAllCommandHandlers" /> methods
	///     when there is a command without corresponding command handler.
	/// </summary>
	[Serializable]
	public class CommandHandlerNotFoundException : InvalidOperationException
	{
		protected internal CommandHandlerNotFoundException ()
		{
		}


		public CommandHandlerNotFoundException (Type commandType, Type resultType)
		{
			this.CommandType = commandType;
			this.ResultType = resultType;
		}


		public Type CommandType { get; protected internal set; }
		public Type ResultType { get; protected internal set; }


		/// <summary>
		///     Gets a message that describes the current exception.
		/// </summary>
		/// <returns>
		///     The error message that explains the reason for the exception, or an empty string ("").
		/// </returns>
		public override string Message
		{
			get
			{
				var message = string.Format ("A command {0} => {1} has no command handler",
				                             this.CommandType.FullName,
				                             this.ResultType.FullName);

				return message;
			}
		}
	}
}