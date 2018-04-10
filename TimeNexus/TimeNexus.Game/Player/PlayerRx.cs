using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.Player
{
	public static class PlayerRx
	{
		public static bool IsPlayer(Collision collision)
		{
			return IsPlayer(collision.ColliderA?.Entity) || IsPlayer(collision.ColliderB?.Entity);
		}

		public static bool IsPlayer(Entity entity)
		{
			return entity?.Get<CharacterComponent>() != null;
		}
	}
}
