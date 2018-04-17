using SiliconStudio.Core;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.Player;

namespace TimeNexus.Objects
{
	public partial class LookatTrigger : ScriptComponent
	{
		private static RaycastDistributor _raycastDistributor = new RaycastDistributor(PlayerRaycaster.OnRaycast);
		[DataMemberIgnore]
		private readonly Subject<HitResult> _onLookatStart = new Subject<HitResult>();
		[DataMemberIgnore]
		private readonly Subject<HitResult> _onLookatStay = new Subject<HitResult>();
		[DataMemberIgnore]
		private readonly Subject<HitResult> _onLookatEnd = new Subject<HitResult>();

		[DataMemberIgnore]
		public IObservable<HitResult> OnLookatStart
		{
			get => _onLookatStart;
		}

		[DataMemberIgnore]
		public IObservable<HitResult> OnLookatStay
		{
			get => _onLookatStay;
		}

		[DataMemberIgnore]
		public IObservable<HitResult> OnLookatEnd
		{
			get => _onLookatEnd;
		}

		public override void Cancel()
		{
			_onLookatStart.OnCompleted();
			_onLookatStay.OnCompleted();
			_onLookatEnd.OnCompleted();
		}
	}
}
