using System;
using System.Linq;
using SiliconStudio.Core;

namespace TimeNexus.Time
{
	/// <summary>
	/// A readonly class!
	/// </summary>
	[Display("Time")]
	public class Time
	{
		protected Era _era = Era.Modern;
		public Time()
		{

		}

		public Time(Era era) : this()
		{
			_era = era;
		}

		/// <summary>
		/// The current Era
		/// </summary>
		public Era Era { get => _era; }

		public Time GetNext()
		{
			var eraArray = Enum.GetValues(typeof(Era));
			//This doesn't look very pretty......but it (probably) works
			return new Time((Era)eraArray.GetValue(Math.Min(Array.IndexOf(eraArray, this.Era) + 1, eraArray.Length - 1)));
		}

		public Time GetPrevious()
		{
			var eraArray = Enum.GetValues(typeof(Era));
			return new Time((Era)eraArray.GetValue(Math.Max(Array.IndexOf(eraArray, this.Era) - 1, 0)));
		}

		public int ToNumber()
		{
			return (int)Era;
		}
	}

}