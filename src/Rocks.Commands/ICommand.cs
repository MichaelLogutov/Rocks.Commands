namespace Rocks.Commands
{
	/// <summary>
	///     A command with specified result type.
	///     For no result use <see cref="NoResult" /> type (return <see cref="NoResult.Void" /> from the handler).
	/// </summary>
	/// <typeparam name="TResult">
	///     Result type. For no result use <see cref="NoResult" /> type (return
	///     <see cref="NoResult.Void" /> from the handler).
	/// </typeparam>
	// ReSharper disable once UnusedTypeParameter
	public interface ICommand<TResult>
	{
	}
}