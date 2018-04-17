using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Core;

namespace TimeNexus.Objects
{
	public class LoadingIconComponent : SyncScript
	{
	
		public SpriteComponent LoadingIcon { get; set; }
		// Declared public member fields and properties will show in the game studio

		[DataMemberIgnore]
		private Quaternion rotation;
		public override void Start()
		{
			rotation = Quaternion.RotationZ(0.03f);
		}

		public override void Update()
		{
			LoadingIcon.Entity.Transform.Rotation *= rotation;

		}
	}
}
