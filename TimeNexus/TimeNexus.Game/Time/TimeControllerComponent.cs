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
using TimeNexus.LevelManagement;
using Reactive.Bindings;

namespace TimeNexus.Time
{

	[Display("Time Controller Component", Expand = ExpandRule.Once)]
	[DataContract("TimeControllerComponent")]
	public class TimeControllerComponent : StartupScript
	{
		private Entity previousEntity;

		[DataMemberIgnore]
		public ReactivePropertySlim<Time> Time { get; } = new ReactivePropertySlim<Time>();

		public override void Start()
		{
			foreach (Entity e in GetChildrenWithTime())
			{
				e.Get<ModelComponent>().Enabled = false;
			}

			Time.Value = this.GetLevel().Settings.DefaultTime;
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
			if(previousEntity != null)
			{
				foreach(var component in previousEntity.GetAll<ActivableEntityComponent>())
				{
					component.Enabled = false;
				}
			}

			if (minEntity != null)
			{
				foreach (var component in minEntity.GetAll<ActivableEntityComponent>())
				{
					component.Enabled = true;
				}
			}
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
	}
}
