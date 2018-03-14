using SiliconStudio.Core;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.Player;

namespace TimeNexus.Objects
{
	public class LookatTrigger : StartupScript, IPlayerTrigger
	{
		public event TriggerEventArgs OnTrigger;
		public event TriggerEventArgs OnTriggerEnd;

		[DataMemberIgnore]
		private bool _previousHit = false;
		public override void Start()
		{
			PlayerRaycaster.OnRaycast += (player, hitResult) =>
			{
				bool hit = hitResult.Succeeded;

				if (hit == _previousHit) return;
				else hit = _previousHit;

				//The hit entity has changed
				if (hitResult.Collider?.Entity == null)
				{
					OnTriggerEnd?.Invoke(this.Entity, player);
				}
				else if (hitResult.Collider?.Entity == this.Entity)
				{
					OnTrigger?.Invoke(this.Entity, player);
				}
			};
		}

	}
}
