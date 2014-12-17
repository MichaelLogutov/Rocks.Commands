namespace Rocks.Commands
{
	/// <summary>
	///     Represents an arbitrary decorator.
	/// </summary>
	public interface IDecorator
	{
		/// <summary>
		///     A decorated object.
		/// </summary>
		object Decorated { get; }
	}
}