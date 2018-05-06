using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.Time;

namespace TimeNexus.LevelManagement
{
	/// <summary>
	/// Level settings
	/// </summary>
	public class LevelSettings : ScriptComponent
	{
		/// <summary>
		/// The default time of this level
		/// </summary>
		public GameStudioTime DefaultTime { get; } = new GameStudioTime();
	}
}
