using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.Levels.Management;

namespace TimeNexus.Levels.Management
{
	public delegate string LevelChooser(Level previous, Gateway gateway);
	public static class LevelChoosers
	{
		public static readonly LevelChooser Default = (level, gateway) =>
		{

			return "";
		};
	}
}
