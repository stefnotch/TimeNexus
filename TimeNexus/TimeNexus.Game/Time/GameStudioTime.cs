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
	[DataContract("GameStudioTime")]
	[Display("GameStudioTime")]
	public class GameStudioTime : Time
	{
		public delegate void TimeChanged(Time t);
		public event TimeChanged OnTimeChanged;

		/// <summary>
		/// The current Era
		/// </summary>
		public new Era Era
		{
			get => _era;
			set
			{
				_era = value;
				OnTimeChanged?.Invoke(this);
			}
		}
	}
}
