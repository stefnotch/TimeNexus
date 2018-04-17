using System;
using System.Linq;
using SiliconStudio.Core;

namespace TimeNexus.Time
{
	/// <summary>
	/// A readonly class!
	/// </summary>
	[Display("Time")]
	[DataContract]
	public class Time
	{
		[DataMember]
		protected Era _era = Era.Modern;

		private static readonly Era[] eraArray = Enum.GetValues(typeof(Era)).Cast<Era>().OrderBy(era => (int)era).ToArray();

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
			int eraIndex = Array.IndexOf(eraArray, _era);
			return new Time(eraArray[Math.Min(eraIndex + 1, eraArray.Length - 1)]);
		}

		public Time GetPrevious()
		{
			int eraIndex = Array.IndexOf(eraArray, _era);
			return new Time(eraArray[Math.Max(eraIndex - 1, 0)]);
		}

		public int ToNumber()
		{
			return (int)Era;
		}
	}

}