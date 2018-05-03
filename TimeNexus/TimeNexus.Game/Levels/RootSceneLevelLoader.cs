using TimeNexus.LevelManagement;
using SiliconStudio.Core;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.IO;
using SiliconStudio.Core.MicroThreading;
using SiliconStudio.Core.Serialization.Contents;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Engine.Design;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.Objects;

namespace TimeNexus.Levels
{
	public class RootSceneLevelLoader : StartupScript
	{
		public string DefaultLevelUrl { get; set; }

		public override void Start()
		{
			if (DefaultLevelUrl == null || DefaultLevelUrl == "") return;

			LevelManager.Instance.LoadLevel(DefaultLevelUrl)
				.ContinueWith(t => LevelManager.Instance.PlaceLevel(t.Result));
		}
	}
}