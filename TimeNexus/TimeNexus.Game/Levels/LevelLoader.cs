﻿using TimeNexus.LevelManagement;
using SiliconStudio.Core;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.IO;
using SiliconStudio.Core.MicroThreading;
using SiliconStudio.Core.Serialization.Contents;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Engine.Design;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.Objects;

namespace TimeNexus.Levels
{
	public enum LevelType
	{
		Corridor,
		Level
	}

	//[AllowMultipleComponents]
	//[RequireComponent(typeof(Gateway))]
	/// <summary>
	/// Loads a level when a player is inside the trigger volume
	/// </summary>
	public class LevelLoader : StartupScript
	{
		private const String CorridorBaseURL = "Corridors";
		private const String LevelBaseURL = "Levels";

		[DataMemberIgnore]
		private Random _rng = new Random();
		[DataMemberIgnore]
		private List<string> _corridorUrls;
		[DataMemberIgnore]
		private List<string> _levelUrls;

		/// <summary>
		/// The type of the level which will get loaded (corridor, actual level, etc.)
		/// </summary>
		public LevelType LevelType { get; set; } = LevelType.Level;

		/// <summary>
		/// The trigger volume, if a player enters is, a level will get loaded
		/// </summary>
		public RigidbodyComponent TriggerVolume { get; set; }
		//[Display(category: "Name")]
		//[NotNull]
		public Gateway Gateway { get; set; }

		public override void Start()
		{
			_corridorUrls = this.GetAssetUrls(CorridorBaseURL);
			_levelUrls = this.GetAssetUrls(LevelBaseURL);

			new NearbyTrigger(TriggerVolume).OnTriggerStart.Subscribe(_ => LoadLevelAsync());
		}

		private async Task LoadLevelAsync()
		{

			if (Gateway.AttachedLevel != null || Gateway.IsLoading) return;

			string levelName;
			switch(LevelType)
			{
				case LevelType.Level:
					levelName = GetRandomLevelName() ?? GetRandomCorridorName();
					break;
				case LevelType.Corridor:
					levelName = GetRandomCorridorName() ?? GetRandomLevelName();
					break;
				default:
					levelName = GetRandomLevelName();
					break;
			}

			await Gateway.AttachLevel(levelName).ConfigureAwait(false);
		}

		private String GetRandomLevelName()
		{
			if (_levelUrls.Count == 0) return null;
			return _levelUrls[_rng.Next(_levelUrls.Count)];
		}

		private String GetRandomCorridorName()
		{
			if (_corridorUrls.Count == 0) return null;
			return _corridorUrls[_rng.Next(_corridorUrls.Count)];
		}


		private List<string> GetAssetUrls(string path)
		{
			if (path.Length == 0) throw new ArgumentException("Path may not be an empty string");

			if (!path.EndsWith("/")) path += "/";
			var n = ContentManager.FileProvider.ContentIndexMap
				.SearchValues(s => s.Key.StartsWith(path));
			return ContentManager.FileProvider.ContentIndexMap
				.SearchValues(s => s.Key.StartsWith(path))
				.Select(s => s.Key)
				.ToList();
		}
	}
}