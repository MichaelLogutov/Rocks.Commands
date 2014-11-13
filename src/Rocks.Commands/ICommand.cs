namespace Rocks.Commands
{
	/// <summary>
	///     A command.
	/// </summary>
	public interface ICommand
	{
	}


	/// <summary>
	///     A command with specified result type.
	///     For no result use <see cref="Void" /> type (return <see cref="Void.Result" /> from the handler).
	/// </summary>
	/// <typeparam name="TResult">
	///     Result type. For no result use <see cref="Void" /> type (return
	///     <see cref="Void.Result" /> from the handler).
	/// </typeparam>
	// ReSharper disable once UnusedTypeParameter
	public interface ICommand<TResult> : ICommand
	{
	}
}