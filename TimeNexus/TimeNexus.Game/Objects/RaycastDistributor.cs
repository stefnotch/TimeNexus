using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Objects
{
	public partial class LookatTrigger
	{
		//This class can only take care of the case where an entity has 1 LookatTrigger
		private class RaycastDistributor
		{
			private readonly IDisposable disposable;

			private LookatTrigger _previousLookatTrigger;
			private HitResult _previousRay;

			public RaycastDistributor(IObservable<HitResult> raycaster)
			{
				disposable =
				raycaster
					.Subscribe(DistributeRay);
			}

			private void DistributeRay(HitResult ray)
			{
				var currEntity = ray.Collider?.Entity;
				var previousEntity = _previousRay.Collider?.Entity;

				var currlookatTrigger = currEntity?.Get<LookatTrigger>();

				//If the raycast's target has changed
				if (currEntity != previousEntity)
				{
					//Send the raycast-start and raycast-end thingies
					if (currlookatTrigger != null) currlookatTrigger._onLookatStart.OnNext(ray);
					if (_previousLookatTrigger != null) _previousLookatTrigger._onLookatEnd.OnNext(_previousRay);
				}

				//Does the entity care about raycasts?
				if (currlookatTrigger != null)
				{
					currlookatTrigger._onLookatStay.OnNext(ray);
				}

				_previousLookatTrigger = currlookatTrigger;
				_previousRay = ray;

			}


			~RaycastDistributor()
			{
				disposable.Dispose();
			}
		}
	}
}
