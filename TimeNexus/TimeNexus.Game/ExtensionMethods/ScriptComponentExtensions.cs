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

		/// <summary>
		/// Disposes of an IDisposeable when the entity gets removed
		/// </summary>
		public static void DisposeLater(this ScriptComponent script, IDisposable disposable)
		{
			script.Entity.GetOrCreate<Disposer>().Disposables.Add(disposable);
		}
		
		public static void AddTask(this ScriptComponent script, Func<Task> task, long priority = 0L)
		{
			MicroThread t = script.Script.AddTask(task, priority);
			script.Entity.GetOrCreate<TaskDisposer>().Tasks.Add(t);
		}

		private class Disposer : ScriptComponent
		{
			public List<IDisposable> Disposables { get; } = new List<IDisposable>();
			public override void Cancel()
			{
				Disposables.ForEach(d => d.Dispose());
			}
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
