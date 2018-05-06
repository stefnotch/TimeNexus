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
	/// <summary>
	/// The keybindings
	/// </summary>
	public class KeyBindings : StartupScript
	{
		private static Keys interact = Keys.E;
		private static Keys pause = Keys.P;
		private static Keys space = Keys.Space;

		private static Keys forward = Keys.W;
		private static Keys backward = Keys.S;
		private static Keys left = Keys.A;
		private static Keys right = Keys.D;

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

		public static Keys Space
		{
			get { return Enabled ? space : Keys.None; }
			set { space = value; }
		}

		public static Keys Forward
		{
			get { return Enabled ? forward : Keys.None; }
			set { forward = value; }
		}

		public static Keys Backward
		{
			get { return Enabled ? backward : Keys.None; }
			set { backward = value; }
		}

		public static Keys Left
		{
			get { return Enabled ? left : Keys.None; }
			set { left = value; }
		}

		public static Keys Right
		{
			get { return Enabled ? right : Keys.None; }
			set { right = value; }
		}

		public static Vector2 MouseDelta
		{
			get { return Enabled ? _input.MouseDelta : Vector2.Zero; }
		}

		public static float MouseWheelDelta
		{
			get { return Enabled ? _input.MouseWheelDelta : 0; }
		}

		/// <summary>
		/// Used to Enable and Disable the keybindings
		/// </summary>
		public static bool Enabled { get; set; } = true;

		public override void Start()
		{
			if (_input != null) Log.Warning("Multiple Keybindings");
			_input = Input;
		}
	}
}
