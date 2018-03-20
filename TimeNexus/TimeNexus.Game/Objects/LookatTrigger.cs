using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.Player;

namespace TimeNexus.Objects
{
	public class LookatTrigger : StartupScript
	{
		private Entity _previousHit;

		public override void Start()
		{
			/*var boundingSphere = (this.Entity.Get<ModelComponent>()?.Model?.BoundingSphere).GetValueOrDefault(new BoundingSphere(Vector3.Zero, 0.01f));

			var trigger = new RigidbodyComponent()
			{
				IsTrigger = true,
				ProcessCollisions = true,
				//CanCollideWith = CollisionFilterGroupFlags.CharacterFilter,
				//CollisionGroup = CollisionFilterGroups.SensorTrigger,
				IsKinematic = true
			};
			trigger.ColliderShapes.Add(new SphereColliderShapeDesc() { Radius = boundingSphere.Radius * 0.9f, LocalOffset = boundingSphere.Center });
			Entity.Add(trigger);*/


			//What if I have an entity that I can move through? (Trigger)
			//I still want to be able to see the trigger in the game studio!

			//What if the entity doesn't even have a collider? Auto-generate? Game studio...?
			var trigger = this.Entity.Get<PhysicsComponent>();

			PlayerRaycaster.OnRaycast += (player, hitResult) =>
			{
				var hit = hitResult.Collider?.Entity;

				//if (Vector3.Distance(player.Transform.Position, this.Entity.Transform.Position) > InteractionRadius) hit = null;
				if (hit == _previousHit) return;
				else _previousHit = hit;

				//The hit entity has changed:
				if (hit == this.Entity)
				{
					OnTrigger?.Invoke(this.Entity, player);
				}
				else
				{
					OnTriggerEnd?.Invoke(this.Entity, player);
				}
			};
		}

		public async Task<RaycastRay> OnRaycast()
		{

		}

	}
}
