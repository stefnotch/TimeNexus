using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Objects
{
	public delegate void TriggerEventArgs(Entity triggerEntity, Entity player);
	public interface IPlayerTrigger
	{
		event TriggerEventArgs OnTrigger;
		event TriggerEventArgs OnTriggerEnd;
	}
}
