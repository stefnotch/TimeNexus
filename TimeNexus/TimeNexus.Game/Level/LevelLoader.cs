using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Level
{
	public class LevelLoader : AsyncScript
	{
		private const String CorridorBaseURL = "Corridors";
		private const String LevelBaseURL = "Levels";
		public const float AsyncLoadRadius = 2.0f;

		private Scene _corridor;

		private Scene _nextLevel;

		public override async Task Execute()
		{
			var trigger = new StaticColliderComponent()
			{
				IsTrigger = true,
				ProcessCollisions = true,
				CanCollideWith = CollisionFilterGroupFlags.CharacterFilter,
				ColliderShape = new SphereColliderShape(false, AsyncLoadRadius)
			};
			Entity.Add(trigger);

			while (Game.IsRunning)
			{
				Collision collision = await trigger.NewCollision();
				var otherCollider = (trigger == collision.ColliderA) ? collision.ColliderB : collision.ColliderA;
				if (otherCollider.Entity.Get<CharacterComponent>() != null)
				{
					if (_corridor != null) LoadCorridor();
					if (_nextLevel != null) LoadLevel();
				}
			}
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
