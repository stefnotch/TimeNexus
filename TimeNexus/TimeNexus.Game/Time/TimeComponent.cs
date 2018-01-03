using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Core;

namespace TimeNexus.Time
{
	[Display("Time Component")]
	public class TimeComponent : SyncScript
	{
		//Well, C# is pretty cool. :)

		/// <summary>
		/// WARNING: This property should NOT be modified outside the game studio
		/// </summary>
		[DataMember]
		public Time Time { get; set; }

		public override void Update()
		{
		}
	}
}
