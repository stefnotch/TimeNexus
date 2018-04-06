using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levels
{
	public static class EntityComponentExtensions
	{
		/// <summary>
		/// Gets the level of the EntityComponent
		/// </summary>
		/// <param name="entityComponent"></param>
		/// <returns>the level or null</returns>
		public static Level GetLevel(this EntityComponent entityComponent)
		{
			LevelManager.Instance.Levels.TryGetValue(entityComponent.Entity.Scene, out Level level);
			return level;
		}
	}
}
