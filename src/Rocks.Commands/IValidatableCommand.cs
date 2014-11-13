namespace Rocks.Commands
{
	/// <summary>
	///     A command that supports validation before it will be passed to command handler.
	/// </summary>
	public interface IValidatableCommand
	{
		/// <summary>
		///     Performs command parameters validation.
		///     Called before command passed to comand handler.
		///     Throw exceptions in case of errors.
		/// </summary>
		void Validate ();
	}
}