using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Core;
using TimeNexus.Levels;

namespace TimeNexus.Time
{

	[Display("Time Controller Component", Expand = ExpandRule.Once)]
	[DataContract("TimeControllerComponent")]
	public class TimeControllerComponent : SyncScript
	{
		private Time _time = new Time();

		private Entity previousEntity;

		[DataMember]
		public Time Time
		{
			get => _time;
			set
			{
				_time = value;
				TimeChanged(value);
			}
		}

		private void TimeChanged(Time t)
		{
			//A little serialisation guard...when serializing an object, Entity == null which leads to annoying errors
			if (Entity == null) return;

			var timeValue = t.ToNumber();
			Entity minEntity = null;
			var minTimeDelta = int.MaxValue;

			foreach (Entity e in GetChildrenWithTime())
			{
				var entityTime = e.Get<TimeComponent>().Time.ToNumber();
				var timeDelta = Math.Abs(entityTime - timeValue);
				if (timeDelta < minTimeDelta)
				{
					minEntity = e;
					minTimeDelta = timeDelta;
				}
			}
			previousEntity?.Enable<ModelComponent>(false);
			minEntity?.Enable<ModelComponent>(true);
			previousEntity = minEntity;
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
			foreach (Entity e in GetChildrenWithTime())
			{
				e.Get<ModelComponent>().Enabled = false;
			}

			Time = LevelSettings.GetLevelSettings(Entity.Scene).DefaultTime;
		}

		public override void Update()
		{
			//MessageBox(0, "You are watching message box!", "Information", 5);
		}
	}
}
