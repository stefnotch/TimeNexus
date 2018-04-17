using SiliconStudio.Core;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.ExtensionMethods
{
	public static class IDisposableExtensions
	{
		/// <summary>
		/// Disposes of an IDisposeable when the entity gets removed
		/// </summary>
		public static void DisposeLater(this IDisposable disposable, ScriptComponent script)
		{
			DisposeLater(disposable, script.Entity);
		}

		/// <summary>
		/// Disposes of an IDisposeable when the entity gets removed
		/// </summary>
		public static void DisposeLater(this IDisposable disposable, Entity entity)
		{
			entity.GetOrCreate<Disposer>().Disposables.Add(disposable);
		}

		private class Disposer : ScriptComponent
		{
			[DataMemberIgnore]
			public List<IDisposable> Disposables { get; } = new List<IDisposable>();
			public override void Cancel()
			{
				Disposables.ForEach(d => d.Dispose());
			}
		}
	}
}
