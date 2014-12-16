namespace Rocks.Commands
{
	/// <summary>
	///     Represents an async command decorator.
	/// </summary>
	public interface IAsyncDecorator<in TCommand, TResult> where TCommand : IAsyncCommand<TResult>
	{
		/// <summary>
		///     A decorated command handler.
		/// </summary>
		IAsyncCommandHandler<TCommand, TResult> Decorated { get; }
	}
}