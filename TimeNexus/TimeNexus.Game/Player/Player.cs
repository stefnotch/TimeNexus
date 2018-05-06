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
using TimeNexus.Levels;

namespace TimeNexus.Player
{
	public class Player : StartupScript
	{
		public static float DefaultHealth { get; set; } = 100;

		public ReactivePropertySlim<float> HealthP { get; } = new ReactivePropertySlim<float>(DefaultHealth);


		public override void Start()
		{
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
			var lastPosition = Entity.Transform.WorldToLocal(Savepoints.Instance.GetLatest().Entity.Transform.GetWorldPosition());
			Entity.Transform.Position = lastPosition;
			HealthP.Value = DefaultHealth;
		}
	}
}
