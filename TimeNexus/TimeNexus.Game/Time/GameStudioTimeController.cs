using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Core.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace TimeNexus.Time
{
	public class GameStudioTimeController : SyncScript
	{
		[DllImport("User32")]
		public static extern int MessageBox(int Hwnd, string text, string caption, int type);


		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool AllocConsole();


		// Declared public member fields and properties will show in the game studio
		private Time _time;

		private bool x = false;

		public Time Time
		{
			get => _time;
			set
			{
				/*if (!x)
				{
					AllocConsole();
					x = true;
				}*/

				_time = value;

				if (Entity?.Scene?.Entities != null)
				{
					foreach (Entity e in Entity.Scene.Entities)
					{
						var timeController = e.Get<TimeControllerComponent>();
						if (timeController != null) timeController.Time = _time;
						
					}

					/*using (var stream = new StreamWriter(Console.OpenStandardOutput()))
					{
						stream.WriteLine("Yo" + e);
						stream.WriteLine("" + e.FindRoot());
						stream.WriteLine("" + e.GetParent());
						stream.WriteLine("" + e.Tags);
						stream.Flush();
					}*/
				}
				//MessageBox(0, "You are watching message box!", "Information", 5);

			}
		}

		public override void Update()
		{
			// Do stuff every new frame
		}
	}
}
