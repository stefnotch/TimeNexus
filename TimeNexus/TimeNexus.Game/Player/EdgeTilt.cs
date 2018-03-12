using SiliconStudio.Core.Collections;
using SiliconStudio.Core.Diagnostics;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Games;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.Debug;

namespace TimeNexus.Player
{
	/// <summary>
	/// This name isn't optimal...
	/// 
	/// It tilts the entity 
	/// </summary>
	public class EdgeTilt : StartupScript
	{
		const float Radius = 1.0f;
		const int RayCount = 16;

		Vector3[] _offsets = new Vector3[RayCount];

		Simulation _simulation;


		ColliderShape _collider;

		public override void Start()
		{
			//this.GetSimulation().ColliderShapesRendering = true;

			_simulation = this.GetSimulation();

			_collider = new SphereColliderShape(false, 0.2f);

			for (var i = 0; i < RayCount; i++)
			{
				// Offset Vector, from 0 to some place on a circle
				_offsets[i] = Vector3.Transform(
									Vector3.UnitZ * Radius,
									Quaternion.RotationY((i / (float)RayCount) * MathUtil.TwoPi)
							);
			}
		}


		public Vector2 CalculatePitchRoll()
		{
			/*if (Entity.Get<CharacterComponent>()?.IsGrounded == false)
			{
				return Vector2.Zero;
			}*/
			//Hell yeah, I'm going to raycast.

			var averageDirection = Vector3.Zero;

			//This code assumes that Y goes to the sky (instead of using the player's up vector)
			var entityPosition = Entity.Transform.WorldMatrix * Matrix.Translation(0, -0.3f, 0);
			for (var i = 0; i < RayCount; i++)
			{
				//WHAT THE HELL!? WHY DO I NEED   Matrix.RotationY(0.01f)  !!??
				var result = _simulation.ShapeSweep(_collider, entityPosition * Matrix.Translation(_offsets[i]) * Matrix.RotationY(0.01f), entityPosition);

				//DebugText.Print(result.Collider + "", new Int2(10, 10 + 15 * i));

				if (result.Succeeded)
				{
					averageDirection += _offsets[i] * result.HitFraction;
				}
				else
				{

					averageDirection += _offsets[i] * 1;
				}
			}

			//DebugDraw.Line(this, averageDirection * 3.0f, Vector3.Zero);
			return averageDirection.XZ() * 0.1f;
		}
	}
}
