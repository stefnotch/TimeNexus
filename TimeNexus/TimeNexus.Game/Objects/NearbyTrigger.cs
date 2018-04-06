using SiliconStudio.Core;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Objects
{
	[DataContract("NearbyTrigger")]
	[Display("Nearby Trigger")]
	public class NearbyTrigger : PhysicsTriggerComponentBase
	{
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

		[DataMemberIgnore]
		public new bool IsTrigger
		{
			get => base.IsTrigger;
			set => base.IsTrigger = value;
		}

		public NearbyTrigger()
		{
			IsTrigger = true;
			ProcessCollisions = true;
			//Collides with characters
			CanCollideWith = CollisionFilterGroupFlags.CharacterFilter;
			//Is a sensor
			CollisionGroup = CollisionFilterGroups.SensorTrigger;
			//IsKinematic = true;
			Collisions.CollectionChanged += OnCollision;
		}

		private void OnCollision(object sender, SiliconStudio.Core.Collections.TrackingCollectionChangedEventArgs args)
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

		protected override void OnDetach()
		{
			_onTriggerStart.OnCompleted();
			//_onTriggerStay.OnCompleted();
			_onTriggerEnd.OnCompleted();

			Collisions.CollectionChanged -= OnCollision;
		}
	}
}
