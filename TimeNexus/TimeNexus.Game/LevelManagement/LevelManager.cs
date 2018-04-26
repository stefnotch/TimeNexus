using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core;
using System.Collections.Concurrent;

namespace TimeNexus.LevelManagement
{
	public class LevelManager : SyncScript
	{
		public const int DefaultDimension = 0;


		[DataMemberIgnore]
		public Dictionary<Scene, Level> Levels { get; } = new Dictionary<Scene, Level>();


		/// <summary>
		/// Gets the LevelManager
		/// Note: This is NOT a Singleton, it's a root scene ScriptComponent
		/// </summary>
		[DataMemberIgnore]
		public static LevelManager Instance { get; private set; }

		[DataMemberIgnore]
		private readonly ConcurrentQueue<Level> levelsToAdd = new ConcurrentQueue<Level>();

		public override void Start()
		{
			if (Instance != null) Log.Warning("Multiple LevelManagers exist:" + Instance + "\n\n" + this);
			Instance = this;

			var rootScene = this.SceneSystem.SceneInstance.RootScene;
			if (!Levels.ContainsKey(rootScene))
			{
				Levels.Add(rootScene, new Level(rootScene));
			}
		}

		/// <summary>
		/// Loads a scene using a url
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public async Task<Level> LoadLevel(string url)
		{
			var scene = await LoadScene(url).ConfigureAwait(false);

			var level = new Level(scene);
			Levels.Add(level.Scene, level);
			return level;
		}

		//TODO: Very shitty, sometimes causes the GC to go crazy but I have no better idea	
		private async Task<Scene> LoadScene(string url)
		{
			bool alreadyLoaded = Content.IsLoaded(url, true);
			var scene = await Content.LoadAsync<Scene>(url);
			var newScene = new Scene
			{
				Name = scene.Name,
				Offset = scene.Offset
			};

			foreach (Entity e in scene.Entities)
			{
				newScene.Entities.Add(e.Clone());
			}

			if (alreadyLoaded) Content.Unload(url);

			return newScene;
		}

		/// <summary>
		/// Places a level
		/// </summary>
		/// <param name="level"></param>
		/// <returns>if the level's dimension is the default dimension</returns>
		public bool PlaceLevel(Level level)
		{
			levelsToAdd.Enqueue(level);
			//TODO: Improve this
			return false;
			
		}

		/// <summary>
		/// It checks if the scene collides with any of the other scenes and then, figures out which "dimension" the scene should be in
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		private int GetFreeDimension(Level s)
		{
			HashSet<int> collidingDimensions = new HashSet<int>();

			var boundingBox = s.BoundingBox;
			foreach (var level in Levels.Values)
			{
				if (level == s) continue;

				if (level.BoundingBox.Intersects(ref boundingBox))
				{
					collidingDimensions.Add(level.Dimension);
				}
			}

			for (int i = 0; ; i++)
			{
				if (!collidingDimensions.Contains(i)) return i;
			}
		}

		public override void Update()
		{
			if(levelsToAdd.TryDequeue(out Level level))
			{
				this.SceneSystem.SceneInstance.RootScene.Children.Add(level.Scene);
				level.Dimension = GetFreeDimension(level);
			}
		}
	}
}
