using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Rendering;
using TimeNexus.Time;
using SiliconStudio.Xenko.Engine.Processors;

namespace Gun
{
	public class GunController : SyncScript
	{
		// Declared public member fields and properties will show in the game studio
		private Simulation simulation;
		public int RayLength { get; set; } = 20;
		private const float Offset = 0.3f;
		public CameraComponent camera { get; set; }

		public MeshTransparentRenderStageSelector RenderStageSelector { get; } = new MeshTransparentRenderStageSelector();

		private ModelComponent _model;

		public override void Start()
		{
			simulation = this.GetSimulation();
			if (camera == null) Log.Error("No camera attached to the gun script");

			RenderStageSelector.TransparentRenderStage = SceneSystem.GraphicsCompositor.RenderStages.First(renderStage => renderStage.Name == "Transparent");
			RenderStageSelector.RenderGroup = RenderGroupMask.Group10;

			foreach(MeshRenderFeature rootRenderFeature in SceneSystem.GraphicsCompositor.RenderFeatures.OfType<MeshRenderFeature>()){ 

				foreach (TransparentRenderStageSelector renderStageSelector in rootRenderFeature.RenderStageSelectors.OfType<TransparentRenderStageSelector>())
				{
					renderStageSelector.RenderGroup = RenderGroupMask.All & ~RenderGroupMask.Group10;
				}

				//rootRenderFeature.RenderStageSelectors.Add(RenderStageSelector);
			} 


			//SceneSystem.SceneInstance.Processors.Add(new TimeProcessor());
			/*SceneSystem.GraphicsCompositor.RenderFeatures.First() .Add(new MeshTransparentRenderStageSelector()
			{

			})*/
			//SceneSystem.GraphicsCompositor.RenderSystem.AddRenderObject
			//SceneSystem.GraphicsCompositor.RenderFeatures.First().

		}

		public override void Update()
		{
			if (_model != null)
			{
				_model.RenderGroup = RenderGroup.Group0;
			}
			var backBuffer = GraphicsDevice.Presenter.BackBuffer;

			if (Input.GetVirtualButton(0, "Shoot") > 0)
			{

				Matrix invViewProj = Matrix.Invert(camera.ViewProjectionMatrix);
				var sPos = new Vector3(0, 0, 0);
				sPos.Z = 0f;
				var vectorNear = Vector3.Transform(sPos, invViewProj);
				vectorNear /= vectorNear.W;

				// Compute the far (end) point for the raycast
				// It's assumed to have the same projection space (x,y) coordinates and z = 1 (lying on the far plane)
				// We need to unproject it to world space
				sPos.Z = 1f;
				var vectorFar = Vector3.Transform(sPos, invViewProj);
				vectorFar /= vectorFar.W;

				// Raycast from the point on the near plane to the point on the far plane and get the collision result
				var result = simulation.Raycast(vectorNear.XYZ(), vectorFar.XYZ());


				if (result.Succeeded)
				{
					var timeComponent = result.Collider.Entity.Get<TimeControllerComponent>() ?? result.Collider.Entity.GetParent()?.Get<TimeControllerComponent>();
					if (timeComponent != null)
					{
						timeComponent.Time = (DateTime.Now.Millisecond % 10 < 3) ? Time.AncientHistory : ((DateTime.Now.Millisecond % 10 < 6) ? Time.FarFuture : Time.Modern);
					}

					var entity = result.Collider.Entity;
					//entity.Tags.
					//entity.EntityManager.Processors.Add(EntityProcessor);
					//new EntityProcessor<TimeComponent>().
					//EntityManager

					_model = entity?.Get<ModelComponent>();

					if (_model != null)
					{
						_model.RenderGroup = RenderGroup.Group10;
						//_model.RenderGroup = RenderGroup.Group10;
						//_model.Materials.


					}
					new MaterialInstance()
					_model.Model.Materials.First().
					/*if(result.Collider.Entity.Get<ModelComponent>()?.Materials != null)
					{
						foreach(var material in result.Collider.Entity.Get<ModelComponent>()?.Materials.Values)
						{
							//material.Passes.Last().HasTransparency = true;
							material.Descriptor.Attributes.Transparency = 
						}
					}*/
					//result.Collider.Entity.Get<ModelComponent>()?.GetMaterial(0)?.Descriptor
					//result.Collider.Entity.Get<ModelComponent>()?.GetMaterial(0)?.Passes?.First()?.HasTransparency = true;
					//result.Collider.Entity.Get<ModelComponent>().Materials.Add(5, new SiliconStudio.Xenko.Rendering.Material)
				}
			}
		}
	}
}
