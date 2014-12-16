namespace Rocks.Commands
{
	/// <summary>
	///     Represents a command decorator.
	/// </summary>
	public interface IDecorator<in TCommand, out TResult> where TCommand : ICommand<TResult>
	{
		/// <summary>
		///     A decorated command handler.
		/// </summary>
		ICommandHandler<TCommand, TResult> Decorated { get; }
	}
}