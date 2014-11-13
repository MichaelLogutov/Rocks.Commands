using System;
using JetBrains.Annotations;
using SimpleInjector;

namespace Rocks.Commands.Implementation
{
	[UsedImplicitly]
	internal class SimpleInjectorCommandHandlerFactory : ICommandHandlerFactory
	{
		private readonly Container container;


		public SimpleInjectorCommandHandlerFactory ([NotNull] Container container)
		{
			if (container == null)
				throw new ArgumentNullException ("container");

			this.container = container;
		}


		public object GetCommandHandler (Type commandHandlerType)
		{
			return this.container.GetInstance (commandHandlerType);
		}
	}
}