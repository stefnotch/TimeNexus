using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Player
{
	/// <summary>
	/// This name isn't optimal...
	/// 
	/// It tilts the entity 
	/// </summary>
	public class EdgeTilt : SyncScript
	{
		RigidbodyComponent[] triggers = new RigidbodyComponent[2];
		public override void Start()
		{
			this.GetSimulation().ColliderShapesRendering = true;

			var baseSize = new Vector3(0.2f, 0.4f, 0.2f);
			var baseOffset = new Vector3(0, -0.1f, 0);

			//var trigger = Entity.GetOrCreate<RigidbodyComponent>();
			for (var i = 0; i < 2; i++)
			{
				triggers[i] = new RigidbodyComponent
				{
					ProcessCollisions = true,
					IsTrigger = true,
					CanSleep = false,

					//TODO: Can be improved by updating the PhysicsWorldTransform it in the PlayerController (every Update())
					RigidBodyType = RigidBodyTypes.Kinematic,
					IsKinematic = true


				};

				if(i == 0)triggers[i].ColliderShapes.Add(new BoxColliderShapeDesc() { Size = baseSize + new Vector3(0.3f, 0, 0), LocalOffset = baseOffset + new Vector3(0, 0, 0.3f) });
				else triggers[i].ColliderShapes.Add(new BoxColliderShapeDesc() { Size = baseSize + new Vector3(0.3f, 0, 0), LocalOffset = baseOffset + new Vector3(0, 0, -0.3f) });

				this.Entity.Add(triggers[i]);
			}
			
			
			//trigger.ColliderShapes.Add(new BoxColliderShapeDesc() { Size = baseSize + new Vector3(), LocalOffset = new Vector3(0, 0.1f, 0) });
			//trigger.ColliderShapes.Add(new BoxColliderShapeDesc() { Size = baseSize + new Vector3(), LocalOffset = new Vector3(0, 0.1f, 0) });

			
		}

		public override void Update()
		{
			//Hell yeah, I'm going to raycast.
			var rotation = Quaternion.Identity;
			Console.WriteLine("0: " + triggers[0].Collisions.Count);

			Console.WriteLine("1: " + triggers[1].Collisions.Count);
			/*foreach (var collision in trigger.Collisions)
			{
				foreach (var contact in collision.Contacts)
				{
					Console.WriteLine("Dist: " + contact.Distance);
					Console.WriteLine($"Normal: {contact.Normal}");
					Console.WriteLine($"PosOnA: {contact.PositionOnA}");
					Console.WriteLine($"PosOnB: {contact.PositionOnB}");
					//contact.
					//rotation *= Quaternion.BetweenDirections(contact.Normal, -Vector3.UnitY);
				}
			}*/
			this.Entity.Transform.Rotation *= rotation;

		}
	}
}
