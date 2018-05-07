using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeNexus.ExtensionMethods;

namespace TimeNexus.PlayerScripts
{
	public class Savepoints
	{
		[DataMemberIgnore]
		readonly Player _player;

		[DataMemberIgnore]
		private List<Savepoint> _savepoints { get; } = new List<Savepoint>();

		public Savepoints(Player player)
		{
			_player = player;
		}

		public Savepoint Save()
		{
			var savepoint = new Savepoint()
			{
				SceneName = _player.GetLevel().Scene.Name,
				Transform = _player.Entity.Transform.WorldMatrix
			};


			return savepoint;
		}

		public Savepoint GetLatest()
		{
			return _savepoints.Last();
		}



	}
}