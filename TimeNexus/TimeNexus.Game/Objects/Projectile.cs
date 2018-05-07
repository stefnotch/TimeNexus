using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.PlayerScripts;

namespace TimeNexus.Objects
{
	public class Projectile : AsyncScript
	{
		public RigidbodyComponent Collider { get; set; }

		public float Damage { get; set; } = 0f;

		public async override Task Execute()
		{
			while (Game.IsRunning && Collider != null)
			{
				var collision = await Collider.NewCollision();
				var player = collision.ColliderA.Entity.Get<Player>() ?? collision.ColliderB.Entity.Get<Player>();
				if (player != null)
				{
					player.HealthP.Value -= Damage;
				}
			}
		}

	}
}
