using SiliconStudio.Core;
using SiliconStudio.Core.Collections;
using SiliconStudio.Core.Diagnostics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Engine.Design;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.ExtensionMethods;

namespace TimeNexus.Objects
{
	/// <summary>
	/// Triggers events when a player is nearby
	/// 
	/// Not an EntityComponent
	/// It's a wrapper around an EntityComponent (Because I can't inherit from RigidbodyComponent)
	/// </summary>
	public class NearbyTrigger : IDisposable
	{
		[DataMemberIgnore]
		private static Logger Logger = GlobalLogger.GetLogger("NearbyTrigger");

		public RigidbodyComponent RigidbodyComponent { get; }

		[DataMemberIgnore]
		private readonly Subject<Collision> _onTriggerStart = new Subject<Collision>();
		[DataMemberIgnore]
		private readonly Subject<Collision> _onTriggerEnd = new Subject<Collision>();

		/// <summary>
		/// Gets called when a player enters the trigger
		/// </summary>
		[DataMemberIgnore]
		public IObservable<Collision> OnTriggerStart { get => _onTriggerStart; }


		//public IObservable<Collision> OnTriggerStay { get => _onTriggerStay; }

		/// <summary>
		/// Gets called when  a player exits the trigger
		/// </summary>
		[DataMemberIgnore]
		public IObservable<Collision> OnTriggerEnd { get => _onTriggerEnd; }

		public NearbyTrigger(RigidbodyComponent physicsComponent)
		{
			RigidbodyComponent = physicsComponent;
			//Is a trigger with collison events
			RigidbodyComponent.IsTrigger = true;
			RigidbodyComponent.ProcessCollisions = true;
			//Collides with characters
			RigidbodyComponent.CanCollideWith = CollisionFilterGroupFlags.CharacterFilter;
			//Is a sensor
			RigidbodyComponent.CollisionGroup = CollisionFilterGroups.SensorTrigger;
			RigidbodyComponent.IsKinematic = true;
			RigidbodyComponent.Collisions.CollectionChanged += OnCollision;

			this.DisposeLater(RigidbodyComponent.Entity);
		}
		
		private void OnCollision(object sender, TrackingCollectionChangedEventArgs args)
		{
			if (args.Action == NotifyCollectionChangedAction.Add)
			{
				_onTriggerStart.OnNext((Collision)args.Item);
			}
			else if (args.Action == NotifyCollectionChangedAction.Remove)
			{
				_onTriggerEnd.OnNext((Collision)args.Item);
			}
			else
			{
				Logger.Info("args.Action wasn't Add/Remove" + args.Action);
			}
		}


		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_onTriggerStart.OnCompleted();
					//_onTriggerStay.OnCompleted();
					_onTriggerEnd.OnCompleted();

					RigidbodyComponent.Collisions.CollectionChanged -= OnCollision;
				}
				disposedValue = true;
			}
		}
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
		}
		#endregion

	}
}
