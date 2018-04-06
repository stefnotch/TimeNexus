using Levels;
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
using TimeNexus.Objects;

namespace TimeNexus.Levels
{
	//[AllowMultipleComponents]
	//[RequireComponent(typeof(Gateway))]
	public class LevelLoader : StartupScript
	{
		private const String CorridorBaseURL = "Corridors";
		private const String LevelBaseURL = "Levels";

		[DataMemberIgnore]
		private List<string> _corridorUrls;
		[DataMemberIgnore]
		private List<string> _levelUrls;

		/// <summary>
		/// TODO: The NearbyTrigger should actually show up in the gamestudio!!!
		/// </summary>
		//[Display(category: "Name")]
		public NearbyTrigger NearbyTrigger { get; set; }

		//[Display(category: "Name")]
		//[NotNull]
		public Gateway Gateway { get; set; }

		public override void Start()
		{

			if (NearbyTrigger == null)
			{
				NearbyTrigger = new NearbyTrigger();
				this.Log.Error("Dafug? NearbyTrigger shouldn't be null");
			}
			this.Entity.Add(NearbyTrigger);

			_corridorUrls = this.GetAssetUrls(CorridorBaseURL);
			_levelUrls = this.GetAssetUrls(LevelBaseURL);

			NearbyTrigger.OnTriggerStart.Subscribe(_ => StartLoading());
			//NearbyTrigger.OnTriggerEnter += StartLoading;


		}

		public void StartLoading()
		{

			//if (_corridor != null) LoadCorridor();
			//if (_nextLevel != null) LoadLevel();
		}

		private async void LoadCorridor()
		{
			//if (_corridor != null) return;

			//_corridor = await Content.LoadAsync<Scene>($"{CorridorBaseURL}/{GetRandomCorridorName()}");

		}

		private async void LoadLevel()
		{
			//if (_nextLevel != null) return;
			//_nextLevel = await Content.LoadAsync<Scene>($"{LevelBaseURL}/{GetRandomLevelName()}");

			//TODO: Make sure that the levels also get unloaded when they aren't needed anymore!!!!
		}

		private String GetRandomLevelName()
		{
			throw new NotImplementedException();
		}

		private String GetRandomCorridorName()
		{
			throw new NotImplementedException();
		}


		public List<string> GetAssetUrls(string path)
		{ 
			if (path.Length == 0) throw new ArgumentException("Path may not be an empty string");

			if (!path.EndsWith("/")) path += "/";
			return ContentManager.FileProvider.ContentIndexMap
				.SearchValues(s => s.Key.StartsWith(path))
				.Select(s => s.Key)
				.ToList();
		}
	}
}