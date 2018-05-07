using Reactive.Bindings;
using SiliconStudio.Core;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.ExtensionMethods;
using TimeNexus.LevelManagement;
using TimeNexus.Levels;

namespace TimeNexus.PlayerScripts
{
	/// <summary>
	/// A player
	/// </summary>
	public class Player : StartupScript
	{
		public static Entity Instance { get; private set; }

		public static float DefaultHealth { get; set; } = 100;

		public ReactivePropertySlim<float> HealthP { get; } = new ReactivePropertySlim<float>(DefaultHealth);

		public Savepoints Savepoints { get; private set; }

		public override void Start()
		{
			if (Instance != null) Log.Error("Multiple player entities?? OwO");
			Instance = this.Entity;

			Savepoints = new Savepoints(this);

			HealthP.Subscribe(health =>
			{
				if(health <= 0)
				{
					Respawn();
				}
			});
		}

		public void Respawn()
		{
			HealthP.Value = DefaultHealth;
			Entity.Transform.WorldMatrix = Savepoints.GetLatest().Transform;
		}

		public Level GetLevel()
		{
			return LevelManager.Instance.CollidingLevels(Entity.Transform.Position).First();
		}
	}
}
