using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;

namespace Settings
{
	public class KeyboardSetup : StartupScript
	{
		// Declared public member fields and properties will show in the game studio

		public override void Start()
		{
			//Well, Virtual Buttons don't seem to be perfect yet...
			Input.GetVirtualButton(0, "Shoot");

			Input.VirtualButtonConfigSet = Input.VirtualButtonConfigSet ?? new VirtualButtonConfigSet();

			//The 0th button config
			//Every button config can store an arbitrary number of virtual buttons
			var buttonConfig = new VirtualButtonConfig();
			var shoot = new VirtualButtonBinding("Shoot", VirtualButton.Mouse.Left); 

			buttonConfig.Add(shoot);
			Input.VirtualButtonConfigSet.Add(buttonConfig);
		}
	}
}
