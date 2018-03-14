using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.Objects;

namespace TimeNexus.Level
{
	public class LevelLoader : StartupScript
	{
		private const String CorridorBaseURL = "Corridors";
		private const String LevelBaseURL = "Levels";

		private Scene _corridor;
		private Scene _nextLevel;

		private NearbyTrigger _nearbyTrigger;

		public override void Start()
		{
			_nearbyTrigger = this.Entity.GetOrCreate<NearbyTrigger>();
			_nearbyTrigger.InteractionRadius = 2.0f;
			_nearbyTrigger.OnTrigger += StartLoading;
			//_nearbyTrigger.OnTriggerEnd += HMMM...;
		}

		public void StartLoading(Entity triggerEntity, Entity other)
		{

			if (_corridor != null) LoadCorridor();
			if (_nextLevel != null) LoadLevel();
		}
		
		private async void LoadCorridor()
		{
			if (_corridor != null) return;

			_corridor = await Content.LoadAsync<Scene>($"{CorridorBaseURL}/{GetRandomCorridorName()}");

		}

		private async void LoadLevel()
		{
			if (_nextLevel != null) return;
			_nextLevel = await Content.LoadAsync<Scene>($"{LevelBaseURL}/{GetRandomLevelName()}");

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
	}
}
