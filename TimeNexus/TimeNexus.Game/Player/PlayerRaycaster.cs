using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Engine.Events;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Linq;
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
		//Testing out some fancy REACTIVE stuff! OwO

		//I suppose making this static is fine...
		private static Subject<HitResult> _onRaycast = new Subject<HitResult>();
		private static Subject<HitResult> _onRaycastStart = new Subject<HitResult>();
		private static Subject<HitResult> _onRaycastEnd = new Subject<HitResult>();

		[DataMemberIgnore]
		private Simulation _simulation;

		[DataMemberIgnore]
		private HitResult _previousRay;

		/// <summary>
		/// Subscribe to this to get notified of every raycast
		/// </summary>
		public static IObservable<HitResult> OnRaycast
		{
			get => _onRaycast;
		}

		public static IObservable<IObservable<HitResult>> OnRaycastHit
		{
			get => _onRaycast
				.Where(ray => ray.Succeeded)
				.GroupBy(ray => ray.Collider.Entity)
				.AsObservable();
		}

		public static IObservable<IObservable<HitResult>> OnRaycastStart
		{
			get => _onRaycastStart
				//.Where(ray => ray.Succeeded)
				.GroupBy(ray => ray.Collider.Entity)
				.AsObservable();
		}

		public static IObservable<IObservable<HitResult>> OnRaycastEnd
		{
			get => _onRaycastEnd
				//.Where(ray => ray.Succeeded)
				.GroupBy(ray => ray.Collider.Entity)
				.AsObservable();
		}


		public CameraComponent Camera { get; set; }

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
			var ray = Raycast(Camera);
			var currentEntity = ray.Collider.Entity;
			var previousEntity = _previousRay.Collider.Entity;

			//If the entity has changed
			//One raycast started, one raycast stopped
			if (currentEntity != previousEntity)
			{
				if (currentEntity != null) _onRaycastStart.OnNext(ray);
				if (previousEntity != null) _onRaycastEnd.OnNext(_previousRay);

				_previousRay = ray;
			}

			_onRaycast.OnNext(ray);
		}

		public override void Cancel()
		{
			_onRaycast.Dispose();
			_onRaycastStart.Dispose();
			_onRaycastEnd.Dispose();
		}
	}
}
