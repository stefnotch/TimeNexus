using SiliconStudio.Core.Collections;
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

		/*Vector3 baseOffset = new Vector3(0, -0.1f, 0);
		float baseDistance = 0.3f;
		float rayCount = 8;
		*/

		Matrix baseOffset = Matrix.Translation(0, -0.1f, 0.4f);

		readonly FastList<HitResult> collisions = new FastList<HitResult>();
		readonly Vector3 _colliderSize = new Vector3(0.2f, 0.4f, 0.2f);

		ColliderShape _collider;
		Simulation _simulation;

		public override void Start()
		{
			//this.GetSimulation().ColliderShapesRendering = true;


			_simulation = this.GetSimulation();

			_collider = new BoxColliderShape(false, _colliderSize);

		}

		public override void Update()
		{
			//Hell yeah, I'm going to raycast.
			//Actually, I changed my mind. XD
			var rotation = Quaternion.Identity;
			//this.GetSimulation().Raycast()
			//_simulation.ShapeSweepPenetrating(_collider, Matrix.Identity, Matrix.Identity);
			Matrix startMatrix = this.Entity.Transform.WorldMatrix ;
			Matrix rotationMatrix = Matrix.RotationY(180);

			DebugText.Print(
				_simulation.ShapeSweepPenetrating(_collider,
					startMatrix * baseOffset,
					startMatrix * rotationMatrix * baseOffset
				).Count + "",
				
				new Int2(10,10));


			this.Entity.Transform.Rotation *= rotation;

		}
	}
}
