using JetBrains.Annotations;

namespace Rocks.Commands.Tests.Queries
{
	[UsedImplicitly]
	internal class TestQueryHandler : ICommandHandler<TestQuery, int>
	{
		public int Execute (TestQuery query)
		{
			return query.Number + 1;
		}
	}
}