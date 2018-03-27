using SiliconStudio.Core;
using SiliconStudio.Core.Collections;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Objects
{
	/// <summary>
	/// Checks if the player is nearby
	/// </summary>
	public class NearbyTrigger : AsyncScript
	{
		private const float DefaultInteractionRadius = 1.0f;

		private PhysicsComponent _trigger = new RigidbodyComponent()
		{
			IsTrigger = true,
			ProcessCollisions = true,
			CanCollideWith = CollisionFilterGroupFlags.CharacterFilter,
			CollisionGroup = CollisionFilterGroups.SensorTrigger,
			IsKinematic = true
		};

		//TODO: Fill this with a sphere by default!
		//TODO: Check out the Xenko source code (CharacterComponent) --> How do they manage to get the colliders to show up in the game studio!?
		public TrackingCollection<IInlineColliderShapeDesc> ColliderShapes { get => _trigger.ColliderShapes; }

		[DataMemberIgnore]
		private readonly Subject<Collision> _onTriggerStart = new Subject<Collision>();
		//[DataMemberIgnore]
		//private Subject<Collision> _onTriggerStay;
		[DataMemberIgnore]
		private readonly Subject<Collision> _onTriggerEnd = new Subject<Collision>();

		public IObservable<Collision> OnTriggerStart { get => _onTriggerStart; }
		//public IObservable<Collision> OnTriggerStay { get => _onTriggerStay; }
		public IObservable<Collision> OnTriggerEnd { get => _onTriggerEnd; }

		public override async Task Execute()
		{
			//Make sure that it's possible to instantly swap this out with a more efficient variant!
			//(Actually, I have no clue how efficient it is to have a bazillion colliders)

			Entity.Add(_trigger);

			// = new SphereColliderShape(false, DefaultInteractionRadius)


			while (Game.IsRunning)
			{
				Collision collision = await _trigger.NewCollision();
				var otherCollider = (_trigger == collision.ColliderA) ? collision.ColliderB : collision.ColliderA;
				if (otherCollider.Entity.Get<CharacterComponent>() != null)
				{
					_onTriggerStart.OnNext(collision);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
					collision.Ended().ContinueWith(_ => _onTriggerEnd?.OnNext(collision));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				}
			}
		}

		public override void Cancel()
		{
			_onTriggerStart.OnCompleted();
			//_onTriggerStay.OnCompleted();
			_onTriggerEnd.OnCompleted();
		}
	}
}
