namespace Rocks.Commands
{
	/// <summary>
	///     Represents convienient way to declare a command with no return value.
	/// </summary>
	public struct Void
	{
		/// <summary>
		///     Represents a static void result.
		/// </summary>
		public static readonly Void Result = new Void ();
	}
}