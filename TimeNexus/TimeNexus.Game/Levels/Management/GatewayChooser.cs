using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Levels.Management
{
	public delegate Gateway GatewayChooser(Level level);
	public static class GatewayChoosers
	{
		public static readonly GatewayChooser NonExit = (level) =>
		{
			return level.Gateways
					.Where(gateway => !gateway.IsExit) //Find then first non-exit gateway
					.DefaultIfEmpty(level.Gateways.First()) //Or just take the first gateway
					.First(); //Take the first element
		};

		/* Equivalent to:
		public static Gateway NonExit1(Level level)
		{
			return level.Gateways
					.Where(gateway => !gateway.IsExit) //Find then first non-exit gateway
					.DefaultIfEmpty(level.Gateways.First()) //Or just take the first gateway
					.First(); //Take the first element
		}*/



	}
}
