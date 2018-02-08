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
	public class LevelLoaded : StartupScript
	{
		// Declared public member fields and properties will show in the game studio

		public override void Start()
		{
			if (Entity?.Scene?.Entities == null) return;

			var levelSettings = LevelSettings.GetLevelSettings(Entity.Scene);
			foreach (Entity e in Entity.Scene.Entities)
			{
				var timeController = e.Get<TimeControllerComponent>();
				if (timeController != null) timeController.Time = levelSettings?.DefaultTime ?? new Time();

			}

		}
	}
}
