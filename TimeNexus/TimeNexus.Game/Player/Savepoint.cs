using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Player
{
	public class Savepoint
	{
		public string SceneName { get; set; }

		public Matrix Transform { get; set; } 
	}
}
