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
	[DataContract("TimeComponent")]
	[Display("Time Component")]
	public class TimeComponent : EntityComponent
	{
		/// <summary>
		/// Don't touch me! I'm a Game Studio specific thingy/hack!
		/// </summary>
		[DataMember]
		public GameStudioTime GameStudioTime { get; } = new GameStudioTime();

		[DataMemberIgnore]
		public Time Time
		{
			get
			{
				return GameStudioTime;
			}
		}
	}
}
