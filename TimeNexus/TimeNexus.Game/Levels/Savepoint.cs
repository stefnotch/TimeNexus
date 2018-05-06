using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.LevelManagement;

namespace TimeNexus.Levels
{
	public class Savepoint : StartupScript
	{
		public string SceneName { get; private set; }

		public override void Start()
		{
			SceneName = this.GetLevel().Scene.Name;
		}

	}
}
