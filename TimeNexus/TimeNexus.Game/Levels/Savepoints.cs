using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeNexus.ExtensionMethods;
using TimeNexus.Levels;

namespace TimeNexus.Levels
{
	public class Savepoints : StartupScript
	{
		[DataMemberIgnore]
		public static Savepoints Instance { get; private set; }

		public Entity Player { get; set; }

		[DataMemberIgnore]
		private List<Savepoint> _savepoints { get; } = new List<Savepoint>();

		public override void Start()
		{
			if (Instance != null) Log.Warning("Multiple savepoints managers");

			Instance = this;
		}

		public void Save()
		{
			var position = Entity.Transform.GetWorldPosition();
			var offset = Vector3.One * 0.2f;
			var currentLevel = LevelManagement.LevelManager.Instance.CollidingLevels(new BoundingBox(position - offset, position + offset)).First();
			
			var savepointEntity = new Entity("Savepoint");
			currentLevel.Scene.Entities.Add(savepointEntity);

			savepointEntity.Transform.Position = savepointEntity.Transform.WorldToLocal(position);

			Savepoint s = new Savepoint();
			savepointEntity.Add(s);
			_savepoints.Add(s);
		}

		public Savepoint GetLatest()
		{
			return _savepoints.Last();
		}



	}
}