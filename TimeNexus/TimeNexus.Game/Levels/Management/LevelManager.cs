using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core;

namespace TimeNexus.Levels.Management
{
	public class LevelManager : ScriptComponent
	{
		public const int DefaultDimension = 0;

		[DataMemberIgnore]
		public static LevelManager Instance { get; set; }

		[DataMemberIgnore]
		public Dictionary<Scene, Level> Levels { get; } = new Dictionary<Scene, Level>();

		public LevelManager()
		{
			if (Instance != null) Log.Error("You can't have 2 level managers at the same time!\n" + Instance + ":" + this);

			Instance = this;
		}

		/// <summary>
		/// Loads a scene using a url
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public async Task<Level> LoadLevel(string url)
		{
			var level = new Level(await Content.LoadAsync<Scene>(url));
			Levels.Add(level.Scene, level);
			return level;
		}

		/// <summary>
		/// It checks if the scene collides with any of the other scenes and then, figures out which "dimension" the scene should be in
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public int GetFreeDimension(Level s)
		{
			HashSet<int> collidingDimensions = new HashSet<int>();

			var boundingBox = s.BoundingBox;
			foreach (var scene in Levels.Values)
			{
				if (scene == s) continue;

				if (scene.BoundingBox.Intersects(ref boundingBox))
				{
					collidingDimensions.Add(scene.Dimension);
				}
			}

			for (int i = 0; ; i++)
			{
				if (!collidingDimensions.Contains(i)) return i;
			}
		}
	}
}
