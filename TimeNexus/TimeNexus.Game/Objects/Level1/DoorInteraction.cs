using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Core;
using System.Reactive.Linq;

namespace TimeNexus.Objects.Level1
{
	public class DoorInteraction : StartupScript
	{
		// Declared public member fields and properties will show in the game studio
		public LoadingIconComponent LoadingIcon { get; set; }

		public InteractionPopup Interaction { get; set; }

		public override void Start()
		{
			LoadingIcon.Entity.EnableAll(false);
			Interaction.OnInteraction
				.FirstAsync()
				.Subscribe(_ => LoadingIcon.Entity.EnableAll(true));
		}
	}
}
