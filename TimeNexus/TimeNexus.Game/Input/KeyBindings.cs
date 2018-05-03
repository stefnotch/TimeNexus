using SiliconStudio.Core;
using SiliconStudio.Core.Diagnostics;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Games;
using SiliconStudio.Xenko.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Input
{


	/*
	 It took me forever to realize that I didn't have to recreate Xenko Virtual Buttons or Java Keybindings....
	 I can just use a bunch of variables. How come I didn't realize this sooner? XD *facepalm* 
	 ( https://answers.unity.com/questions/374620/setting-up-key-binding-options.html )
		 */
	public class KeyBindings : StartupScript
	{
		private static Keys interact = Keys.E;
		private static Keys pause = Keys.P;

		private static InputManager _input;

		public static Keys Interact
		{
			get { return Enabled ? interact : Keys.None; }
			set { interact = value; }
		}

		public static Keys Pause
		{
			get { return Enabled ? pause : Keys.None; }
			set { pause = value; }
		}

		public static Vector2 MouseDelta
		{
			get { return Enabled ? _input.MouseDelta : Vector2.Zero; }
		}

		public static bool Enabled { get; set; } = true;

		public override void Start()
		{
			if (_input != null) Log.Warning("Multiple Keybindings");
			_input = Input;
		}
	}
}
