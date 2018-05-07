using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Objects
{
	public class ProjectileSpawner : StartupScript
	{
		public Projectile Projectile { get; set; }

		public List<IDisposable> test { get; } = new List<IDisposable>();
	}
}
