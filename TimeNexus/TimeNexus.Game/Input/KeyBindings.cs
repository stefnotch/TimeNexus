using SiliconStudio.Core.Mathematics;
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
	public static class KeyBindings
	{
		
		private static Keys _interact = Keys.E;
		private static Keys _pause = Keys.P;

		public static Keys Interact
		{
			get { return Enabled ? _interact : Keys.None; }
			set { _interact = value; }
		}

		public static Keys Pause
		{
			get { return Enabled ? _pause : Keys.None; }
			set { _pause = value; }
		}

		private static Vector2 MouseDelta
		{
			//get { return Enabled ? //Input.MouseDelta : Vector2; }
			get { return Vector2.Zero; }
		}

		public static bool Enabled { get; set; } = true;
	}
}
