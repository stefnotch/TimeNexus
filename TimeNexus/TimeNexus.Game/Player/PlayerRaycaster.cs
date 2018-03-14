﻿using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Engine.Events;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Player
{
	public delegate void PlayerRaycastEventArgs(Entity player, HitResult hitResult);

	/// <summary>
	/// Attach this script to a player entity!
	/// </summary>
	public class PlayerRaycaster : SyncScript
	{
		[DataMemberIgnore]
		private Simulation _simulation;

		public CameraComponent Camera { get; set; }

		//I suppose making this static is fine...
		/// <summary>
		/// Every time a player-raycast happens, this event is fired
		/// </summary>
		public static event PlayerRaycastEventArgs OnRaycast;

		/// <summary>
		/// If the raycast actually hit something, this event is fired as well
		/// </summary>
		public static event PlayerRaycastEventArgs OnRaycastHit;


		public override void Start()
		{
			_simulation = this.GetSimulation();

			if (Camera == null) Log.Error("No camera attached to the gun script");
		}

		private HitResult Raycast(CameraComponent camera)
		{
			Matrix invViewProj = Matrix.Invert(camera.ViewProjectionMatrix);
			var sPos = new Vector3(0, 0, 0);
			sPos.Z = 0f;
			var vectorNear = Vector3.Transform(sPos, invViewProj);
			vectorNear /= vectorNear.W;

			// Compute the far (end) point for the raycast
			// It's assumed to have the same projection space (x,y) coordinates and z = 1 (lying on the far plane)
			// We need to unproject it to world space
			sPos.Z = 1f;
			var vectorFar = Vector3.Transform(sPos, invViewProj);
			vectorFar /= vectorFar.W;

			// Raycast from the point on the near plane to the point on the far plane and get the collision result
			//It won't collide with sensors
			return _simulation.Raycast(vectorNear.XYZ(), vectorFar.XYZ(), CollisionFilterGroups.AllFilter, CollisionFilterGroupFlags.AllFilter ^ CollisionFilterGroupFlags.SensorTrigger);
		}

		public override void Update()
		{
			var result = Raycast(Camera);

			OnRaycast?.Invoke(Entity, result);
			if (result.Succeeded && result.Collider.Entity != null)
			{
				OnRaycastHit?.Invoke(Entity, result);
			}
		}
	}
}
