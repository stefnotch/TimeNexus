using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Objects
{
	public abstract class ObjectInteraction : AsyncScript
	{
		public float InteractionRadius { get; set; } = 1.0f;

		public sealed override async Task Execute()
		{
			//Make sure that it's possible to instantly swap this out with a more efficient variant!
			//(Actually, I have no clue how efficient it is to have a bazillion colliders)
			var trigger = new RigidbodyComponent()
			{
				IsTrigger = true,
				ProcessCollisions = true,
				CanCollideWith = CollisionFilterGroupFlags.CharacterFilter,
				ColliderShape = new SphereColliderShape(false, InteractionRadius),
				IsKinematic = true
			};
			Entity.Add(trigger);

			while (Game.IsRunning)
			{
				Collision collision = await trigger.NewCollision();
				var otherCollider = (trigger == collision.ColliderA) ? collision.ColliderB : collision.ColliderA;
				if (otherCollider.Entity.Get<CharacterComponent>() != null)
				{
					StartInteraction();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
					collision.Ended().ContinueWith((_) => EndInteraction());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				}
			}
		}
		public abstract void StartInteraction();

		public abstract void EndInteraction();
	}
}
