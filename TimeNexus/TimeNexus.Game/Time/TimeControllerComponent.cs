using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Core;

namespace TimeNexus.Time
{

	[Display("Time Controller Component", Expand = ExpandRule.Once)]
	[DataContract("TimeControllerComponent")]
	public class TimeControllerComponent : SyncScript
	{

		[DataMember]
		private Time _time;

		// Declared public member fields and properties will show in the game studio

		[DataMember]
		public Time Time
		{
			get => _time;
			set
			{
				_time = value;
				//A little serialisation guard...when serializing an object, Entity == null which leads to annoying errors
				if (Entity == null) return;

				var timeValue = (int)_time;
				Entity minEntity = null;
				var minTimeDelta = int.MaxValue;
				
				foreach (Entity e in GetChildrenWithTime())
				{
					var entityTime = (int)e.Get<TimeComponent>().Time;
					var timeDelta = Math.Abs(entityTime - timeValue);
					if (timeDelta < minTimeDelta)
					{
						minEntity = e;
						minTimeDelta = timeDelta;
					}
				}

				foreach (Entity e in GetChildrenWithTime())
				{
					if (e == minEntity) continue;
					e.Enable<ModelComponent>(false);
					e.Get<ModelComponent>().Enabled = false;
				}

				minEntity.Enable<ModelComponent>(true);
			}
		}

		private IEnumerable<Entity> GetChildrenWithTime()
		{
			foreach (Entity e in Entity.GetChildren())
			{
				if (e.Get<TimeComponent>() != null)
				{
					yield return e;
				}
			}
		}

		public override void Start()
		{

		}

		public override void Update()
		{
			//MessageBox(0, "You are watching message box!", "Information", 5);
		}
	}
}
