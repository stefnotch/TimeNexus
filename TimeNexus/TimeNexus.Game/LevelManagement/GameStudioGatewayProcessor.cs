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
			public ModelComponent DebugModel;
		}

		private Scene debugScene;

		private SceneSystem sceneSystem;
		private GameStudioGatewayRenderingService gameStudioGatewayRendering;

		protected override void OnSystemAdd()
		{
			sceneSystem = Services.GetSafeServiceAs<SceneSystem>();

			gameStudioGatewayRendering = Services.GetService<GameStudioGatewayRenderingService>();
			if (gameStudioGatewayRendering == null)
			{
				gameStudioGatewayRendering = new GameStudioGatewayRenderingService(Services);
				var gameSystems = Services.GetSafeServiceAs<IGameSystemCollection>();
				gameSystems.Add(gameStudioGatewayRendering);
			}

			if (debugScene == null)
			{
				debugScene = new Scene();
				//sceneSystem.SceneInstance.RootScene.Children.Add(debugScene);
			}
		}

		protected override void OnSystemRemove()
		{
			if (debugScene != null)
			{
				//Not sure about this:
				//sceneSystem.SceneInstance.RootScene.Children.Remove(debugScene);
				debugScene.Dispose();
			}
		}

		protected override void OnEntityComponentAdding(Entity entity, Gateway component, AssociatedData data)
		{
			if (data.DebugEntity == null) return;
			//debugScene.Entities.Add(data.DebugEntity);
			entity.Add(data.DebugModel);

		}

		protected override void OnEntityComponentRemoved(Entity entity, Gateway component, AssociatedData data)
		{
			if (data.DebugEntity == null) return;
			//debugScene.Entities.Remove(data.DebugEntity);
			entity.Remove(data.DebugModel);
		}

		protected override AssociatedData GenerateComponentData([NotNull] Entity entity, [NotNull] Gateway component)
		{

			var debugEntity = new Entity(entity.Transform.Position)
			{
				new ModelComponent(gameStudioGatewayRendering.CreateArrow())
			};

			return new AssociatedData()
			{
				DebugEntity = debugEntity,
				DebugModel = new ModelComponent(gameStudioGatewayRendering.CreateArrow())
			};

			//MessageBox(0, "You are watching message box", "Information", 5);
		}

		/*public override void Update(GameTime time)
		{

		}*/


	}
}
