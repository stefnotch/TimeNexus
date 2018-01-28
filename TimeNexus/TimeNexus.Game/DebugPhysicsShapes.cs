using System.Linq;
using System.Threading.Tasks;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Xenko.Rendering;
using SiliconStudio.Xenko.Rendering.Compositing;

namespace TimeNexus
{
	public class DebugPhysicsShapes : AsyncScript
	{
		public RenderGroup RenderGroup = RenderGroup.Group7;

		public override async Task Execute()
		{
			//setup rendering in the debug entry point if we have it
			var compositor = SceneSystem.GraphicsCompositor;
			var forwardRenderer =
				 ((compositor.Game as SceneCameraRenderer)?.Child as SceneRendererCollection)?.Children.OfType<ForwardRenderer>().FirstOrDefault();
			if (forwardRenderer == null)
				return;

			var shapesRenderState = forwardRenderer.TransparentRenderStage;//new RenderStage("PhysicsDebugShapes", "Main");
			compositor.RenderStages.Add(shapesRenderState);
			var meshRenderFeature = compositor.RenderFeatures.OfType<MeshRenderFeature>().First();
			meshRenderFeature.RenderStageSelectors.Add(new SimpleGroupToRenderStageSelector
			{
				EffectName = "Shader1",
				RenderGroup = (RenderGroupMask)(1 << (int)RenderGroup),
				RenderStage = shapesRenderState,
			});

			//meshRenderFeature.PipelineProcessors.Add(new PipelineProcessor { RenderStage = shapesRenderState });
			//forwardRenderer.DebugRenderStages.Add(shapesRenderState);

			var simulation = this.GetSimulation();
			if (simulation != null)
				simulation.ColliderShapesRenderGroup = RenderGroup;

			var enabled = false;
			while (Game.IsRunning)
			{
				if (Input.IsKeyReleased(Keys.F3))
				{
					if (simulation != null)
					{
						if (enabled)
						{
							simulation.ColliderShapesRendering = false;
							enabled = false;
						}
						else
						{
							simulation.ColliderShapesRendering = true;
							enabled = true;
						}
					}
				}

				await Script.NextFrame();
			}
		}
	}
}
