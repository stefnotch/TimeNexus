using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using TimeNexus.Time;
using SiliconStudio.Core;

namespace TimeNexus.Levels
{
	public class LevelSettings : StartupScript
	{
		private static readonly PropertyKey<LevelSettings> levelSettingsPropertyKey = new PropertyKey<LevelSettings>("LevelSettings", typeof(LevelSettings));
		private static readonly LevelSettings defaultLevelSettings = new LevelSettings(); //TODO: Make this readonly

		//Add the level settings here!
		public GameStudioTime DefaultTime { get; } = new GameStudioTime();


		/// <summary>
		/// Adds the level settings to the scene
		/// </summary>
		public override void Start()
		{
			Entity?.Scene.Tags.Add(levelSettingsPropertyKey, this);
		}

		//TODO: Turn this into an extension method
		/// <summary>
		/// Gets the level settings 
		/// Tip: You can use "this.Entity.Scene" to get the current scene
		/// </summary>
		/// <param name="s">the scene</param>
		public static LevelSettings GetLevelSettings(Scene s)
		{
			LevelSettings levelSettings = null;
			if (s.Tags.TryGetValue(levelSettingsPropertyKey, out levelSettings))
			{
				return levelSettings;
			}
			else
			{
				return defaultLevelSettings;
			}
		}
	}
}
