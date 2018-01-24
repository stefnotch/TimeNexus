using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using TimeNexus.Time;

namespace Level
{
	public class LevelSettings : StartupScript
	{
		// Declared public member fields and properties will show in the game studio
		public static LevelSettings GetLevelSettings(Scene s)
		{
			return s.Entities.First(e => e.Get<LevelSettings>() != null)?.Get<LevelSettings>();
		}

		public GameStudioTime DefaultTime { get; } = new GameStudioTime();

		public override void Start()
		{
			//Entity.Scene
		}
	}

	public static class LevelSetting
	{

	}
}
