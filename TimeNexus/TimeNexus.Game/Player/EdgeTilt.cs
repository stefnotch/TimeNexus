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

namespace TimeNexus.Player
{
	/// <summary>
	/// This name isn't optimal...
	/// 
	/// It tilts the entity 
	/// </summary>
	public class EdgeTilt : StartupScript
	{
		const float radius = 0.5f;
		const int rayCount = 16;

		Vector3[] offsets = new Vector3[rayCount];

		Simulation _simulation;


		ColliderShape _collider;
		public override void Start()
		{
			//this.GetSimulation().ColliderShapesRendering = true;

			_simulation = this.GetSimulation();

			_collider = new BoxColliderShape(false, new Vector3(0.1f, 0.1f, 0.1f));

			for (var i = 0; i < rayCount; i++)
			{
				// Offset Vector, from 0 to some place on a circle

				offsets[i] = Vector3.Transform(
									Vector3.UnitZ,
									Quaternion.RotationY((i / rayCount) * MathUtil.TwoPi)
							) * radius;
			}
		}


		public Vector2 CalculatePitchRoll()
		{
			if (Entity.Get<CharacterComponent>()?.IsGrounded == false)
			{
				return Vector2.Zero;
			}
			//Hell yeah, I'm going to raycast.
			//Ok, it's kinda a perf killer but whatever

			//

			var averageDirection = Vector3.Zero;


			//This code assumes that Y goes to the sky (instead of using the player's up vector)
			var entityPosition = Entity.Transform.WorldMatrix * Matrix.Translation(0, -0.3f, 0);
			for (var i = 0; i < rayCount; i++)
			{
				// Offset Vector, from 0 to some place on a circle
				
				var result = _simulation.ShapeSweep(_collider, entityPosition * Matrix.Translation(offsets[i]), entityPosition);

				if (result.Succeeded)
				{

					averageDirection += offsets[i] * result.HitFraction;
				}
			}




			return averageDirection.XZ() * 0.2f;
		}
	}
}
