using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core;

namespace Levels
{
	public class LevelManager : ScriptComponent
	{
		public const int DefaultDimension = 0;

		[DataMemberIgnore]
		private static LevelManager _instance;
		[DataMemberIgnore]
		public static LevelManager Instance
		{
			get
			{
				if (_instance == null) _instance = new LevelManager();
				return _instance;
			}
		}

		[DataMemberIgnore]
		public Dictionary<Scene, Level> Levels { get; } = new Dictionary<Scene, Level>();

		/// <summary>
		/// Loads a scene using a url
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public async Task<Level> LoadLevel(string url)
		{
			var scene = Content.Load<Scene>(url);//.ConfigureAwait(false);
			var level = new Level(scene);
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
