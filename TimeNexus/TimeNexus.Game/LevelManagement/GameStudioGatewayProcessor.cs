using SiliconStudio.Core;
using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Extensions;
using SiliconStudio.Xenko.Games;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Graphics.GeometricPrimitives;
using SiliconStudio.Xenko.Rendering;
using SiliconStudio.Xenko.Rendering.Materials;
using SiliconStudio.Xenko.Rendering.Materials.ComputeColors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNexus.LevelManagement
{
	public class GameStudioGatewayProcessor : EntityProcessor<Gateway, GameStudioGatewayProcessor.AssociatedData>
	{
		public class AssociatedData
		{
			public Entity DebugEntity;
		}

		private List<Entity> debugEntities = new List<Entity>();
		private GameStudioGatewayRenderingService gameStudioGatewayRendering;
		private readonly Quaternion DefaultRotation = Quaternion.RotationX(MathUtil.PiOverTwo);

		protected override void OnSystemAdd()
		{
			gameStudioGatewayRendering = Services.GetService<GameStudioGatewayRenderingService>();
			if (gameStudioGatewayRendering == null)
			{
				gameStudioGatewayRendering = new GameStudioGatewayRenderingService(Services);
				var gameSystems = Services.GetSafeServiceAs<IGameSystemCollection>();
				gameSystems.Add(gameStudioGatewayRendering);
			}
		}

		protected override void OnEntityComponentAdding(Entity entity, Gateway component, AssociatedData data)
		{
			if (data.DebugEntity == null) return;
			entity.AddChild(data.DebugEntity);
			debugEntities.Add(data.DebugEntity);

		}

		protected override void OnEntityComponentRemoved(Entity entity, Gateway component, AssociatedData data)
		{
			if (data.DebugEntity == null) return;
			entity.RemoveChild(data.DebugEntity);
			debugEntities.Remove(data.DebugEntity);
		}

		protected override AssociatedData GenerateComponentData([NotNull] Entity entity, [NotNull] Gateway component)
		{

			var debugEntity = new Entity("GameStudioGatewayArrow")
			{
				new ModelComponent(gameStudioGatewayRendering.CreateArrow())
			};

			debugEntity.Transform.Rotation = DefaultRotation * component.Rotation;

			return new AssociatedData()
			{
				DebugEntity = debugEntity,
			};

			//MessageBox(0, "You are watching message box", "Information", 5);
		}

		public override void Update(GameTime time)
		{
			foreach(Entity e in debugEntities)
			{
				Quaternion? rotation = e.GetParent()?.Get<Gateway>()?.Rotation;
				if (rotation.HasValue)
				{
					e.Transform.Rotation = DefaultRotation * rotation.Value;
				}
			}
		}


	}
}
