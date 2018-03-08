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
	public class EdgeTilt : StartupScript
	{
		float radius = 0.5f;
		float rayCount = 16;
		float rayLength = 0.5f;


		Simulation _simulation;


		ColliderShape _collider;
		public override void Start()
		{
			//this.GetSimulation().ColliderShapesRendering = true;


			_simulation = this.GetSimulation();

			_collider = new BoxColliderShape(false, new Vector3(0.7f, 0.1f, 0.7f));
		}

		public Vector2 CalculatePitchRoll()
		{
			//Hell yeah, I'm going to raycast.
			//Ok, it's kinda a perf killer but whatever




			Matrix startMatrix = this.Entity.Transform.WorldMatrix * Matrix.Translation(0,-0.3f,0f);//0,-0.2f,-1f

			//Straight line
			/*DebugText.Print(
			  _simulation.ShapeSweepPenetrating(_collider,
				startMatrix,
				startMatrix * Matrix.Translation(0,0,2f)
			  ).Count + "",

			  new Int2(10, 10));
			  */
			int x = 0;
			for(int i = 0; i < 100; i++)
			{
				/*x += _simulation.ShapeSweepPenetrating(_collider,
				startMatrix,
				startMatrix * Matrix.RotationY(MathUtil.DegreesToRadians(180))
			  ).Count;*/
				
				//x+=(int)_simulation.Raycast(this.Entity.Transform.Position, this.Entity.Transform.Position - Vector3.UnitY * 0.5f).Point.X;
			}
			//Sweep
			DebugText.Print(
			  _simulation.ShapeSweepPenetrating(_collider,
				startMatrix,
				startMatrix * Matrix.RotationY(MathUtil.DegreesToRadians(180))
			  ).Count +x + "",

			  new Int2(10, 10));
			var averageDirection = Vector3.Zero;
			/*
			var entityPosition = Entity.Transform.WorldMatrix.TranslationVector;
			var downVector = -Vector3.UnitY * rayLength;
			for (var i = 0; i < rayCount; i++)
			{
				// Offset Vector, from 0 to some place on a circle
				//This code assumes that Y goes to the sky (instead of using the player's up vector)
				var offset = Vector3.Transform(
									Vector3.UnitZ,
									Quaternion.RotationY(i * rayCount / MathUtil.TwoPi)
							) * radius;

				var result = _simulation.Raycast(entityPosition + offset + Vector3.UnitY * 0.2f, entityPosition + offset + downVector);

				if(!result.Succeeded)
				{
					averageDirection += offset;
				}
			}
			
			*/



			return averageDirection.XZ() * 0.2f;
		}
	}
}
