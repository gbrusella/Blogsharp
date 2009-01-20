using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogSharp.Core
{
	public static class EntityFactory<T>
	{
		public static IEntityFactory<T> Instance
		{
			get
			{
				return DI.Container.Resolve<IEntityFactory<T>>();
			}
		}
	}
}
