using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.Time;

namespace TimeNexus.LevelManagement
{
	public class LevelSettings : ScriptComponent
	{
		public GameStudioTime DefaultTime { get; } = new GameStudioTime();
	}
}
