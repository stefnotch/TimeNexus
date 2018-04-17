using SiliconStudio.Core.MicroThreading;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.ExtensionMethods
{
	public static class ScriptComponentExtensions
	{	
		public static void AddTask(this ScriptComponent script, Func<Task> task, long priority = 0L)
		{
			MicroThread t = script.Script.AddTask(task, priority);
			script.Entity.GetOrCreate<TaskDisposer>().Tasks.Add(t);
		}

		private class TaskDisposer : ScriptComponent
		{
			public List<MicroThread> Tasks { get; } = new List<MicroThread>();
			public override void Cancel()
			{
				Tasks.ForEach(task => task.Cancel());
			}
		}
	}
}
