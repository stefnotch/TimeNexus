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
using SiliconStudio.Xenko.Rendering.Materials;
using SiliconStudio.Xenko.Rendering.Materials.ComputeColors;
using SiliconStudio.Xenko.Shaders;

namespace Gun
{
	public class GunController : SyncScript
	{
		// Declared public member fields and properties will show in the game studio
		private Simulation simulation;
		public CameraComponent camera { get; set; }

		//public MeshTransparentRenderStageSelector RenderStageSelector { get; } = new MeshTransparentRenderStageSelector();

		private ModelComponent _selectedModel;

		public Material effectMaterial;

		public override void Start()
		{
			simulation = this.GetSimulation();
			if (camera == null) Log.Error("No camera attached to the gun script");

			/*
			RenderStageSelector.TransparentRenderStage = SceneSystem.GraphicsCompositor.RenderStages.First(renderStage => renderStage.Name == "Transparent");
			RenderStageSelector.RenderGroup = RenderGroupMask.Group10;

			foreach(MeshRenderFeature rootRenderFeature in SceneSystem.GraphicsCompositor.RenderFeatures.OfType<MeshRenderFeature>()){ 

				foreach (TransparentRenderStageSelector renderStageSelector in rootRenderFeature.RenderStageSelectors.OfType<TransparentRenderStageSelector>())
				{
					renderStageSelector.RenderGroup = RenderGroupMask.All & ~RenderGroupMask.Group10;
				}

				//rootRenderFeature.RenderStageSelectors.Add(RenderStageSelector);
			} */
		}

		private HitResult Raycast(CameraComponent camera)
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
			return simulation.Raycast(vectorNear.XYZ(), vectorFar.XYZ());
		}

		public override void Update()
		{
			var result = Raycast(camera);
			if (!result.Succeeded || result.Collider.Entity == null) return;


			var entity = result.Collider.Entity;

			if (Input.MouseWheelDelta != 0)
			{
				var timeComponent = entity.Get<TimeControllerComponent>() ?? entity.GetParent()?.Get<TimeControllerComponent>();
				if (timeComponent != null)
				{
					if (Input.MouseWheelDelta < 0) timeComponent.Time = timeComponent.Time.GetPrevious();
					else timeComponent.Time = timeComponent.Time.GetNext();
				}
			}

			var newModel = entity.Get<ModelComponent>();
			if (newModel != null && _selectedModel != newModel)
			{
				_selectedModel = newModel;
			}


			if (Input.HasDownMouseButtons) {
				if (entity?.Get<ModelComponent>() != null)
				{
					_selectedModel = entity?.Get<ModelComponent>();
					_selectedModel.RenderGroup = RenderGroup.Group7;

					entity.Remove<ModelComponent>();
					entity.Add(_selectedModel);
				}
				foreach(Entity e in entity.GetChildren())
				{
					if (e?.Get<ModelComponent>() != null && e.Get<ModelComponent>().Enabled)
					{
						_selectedModel = e?.Get<ModelComponent>();
						_selectedModel.RenderGroup = RenderGroup.Group7;

						e.Remove<ModelComponent>();
						e.Add(_selectedModel);
					}
				}
			}
			return;

			//entity.Add(_model);

			if (entity?.Get<ModelComponent>() != null)
			{
				_selectedModel = entity?.Get<ModelComponent>();
				//var clonedModel = _model.Model.Instantiate();

				for (int i = 0; i < _selectedModel.GetMaterialCount(); i++)
				{
					var clonedMaterial = new Material();
					foreach (var pass in _selectedModel.GetMaterial(i).Passes)
					{
						var clonedPass = new MaterialPass(pass.Parameters);
						clonedPass.HasTransparency = pass.HasTransparency;
						clonedPass.BlendState = pass.BlendState;
						clonedPass.CullMode = pass.CullMode;
						clonedPass.IsLightDependent = pass.IsLightDependent;
						//ShaderMixinSource x = (ShaderMixinSource)clonedPass.Parameters.Get(MaterialKeys.PixelStageSurfaceShaders);
						//var y = clonedPass.Parameters.Get(MaterialKeys.PixelStageSurfaceFilter);
						/*foreach(var composition in x.Compositions.Values)
						{
							if(composition is ShaderArraySource)
							{
								ShaderArraySource shaderArraySource = composition as ShaderArraySource;
								((ShaderMixinSource)shaderArraySource.Values[0]).Compositions["diffuseMap"] = (SiliconStudio.Xenko.Shaders.ShaderClassSource)@"DAB";
							}
						}*/
						/*x.AddComposition("SWAG", (SiliconStudio.Xenko.Shaders.ShaderClassSource)@" namespace MyGame
{
shader SWAG : ComputeColor
{
  override float4 Compute()
  {
		return float4(1, 0, 0, 1);
  }

};
}

");*/
						//x.Mixins.Add("hi");// SiliconStudio.Xenko.Shaders.ShaderClassSource
						//SiliconStudio.Xenko.Shaders.ShaderClassSource y = x.Mixins[0];
						//x.Mixins[0] = "";//"mixin MaterialSurfaceArray [{layers = [mixin MaterialSurfaceDiffuse [{diffuseMap = SWAG}], mixin MaterialSurfaceGlossinessMap<false> [{glossinessMap = ComputeColorConstantFloatLink<Material.GlossinessValue>}], mixin MaterialSurfaceMetalness [{metalnessMap = ComputeColorConstantFloatLink<Material.MetalnessValue>}], mixin MaterialSurfaceLightingAndShading [{surfaces = [MaterialSurfaceShadingDiffuseLambert<false>, mixin MaterialSurfaceShadingSpecularMicrofacet [{environmentFunction = MaterialSpecularMicrofacetEnvironmentGGXLUT}, {fresnelFunction = MaterialSpecularMicrofacetFresnelSchlick}, {geometricShadowingFunction = MaterialSpecularMicrofacetVisibilitySmithSchlickGGX}, {normalDistributionFunction = MaterialSpecularMicrofacetNormalDistributionGGX}]]}]]}]";

						//clonedPass.Parameters.Set(MaterialKeys.PixelStageSurfaceShaders, x);
						//ShaderMixinManager.Generate("SWAG");

						//var shaders = new ShaderArraySource();

						//shaders.Add(clonedPass.Parameters.Get(MaterialKeys.PixelStageSurfaceShaders));

						//shaders.Add()
						//clonedPass.Parameters.Set(MaterialKeys.PixelStageSurfaceShaders, shaders);

						clonedMaterial.Passes.Add(clonedPass);


						var clonedPass1 = new MaterialPass(effectMaterial.Passes[0].Parameters);
						clonedPass1.HasTransparency = pass.HasTransparency;
						clonedPass1.BlendState = pass.BlendState;
						clonedPass1.CullMode = pass.CullMode;
						clonedPass1.IsLightDependent = pass.IsLightDependent;

						clonedMaterial.Passes.Add(clonedPass1);
					}

					/*var parCol = new ParameterCollection();
					//parCol.Set(MaterialKeys.PixelStageSurfaceShaders, (SiliconStudio.Xenko.Shaders.ShaderClassSource)"hi");
					parCol.Set(MaterialKeys.DiffuseValue, Color4.White);
					clonedMaterial.Passes.Add(new MaterialPass(parCol)
						{
						HasTransparency = true,
						BlendState = new BlendStateDescription(Blend.One, Blend.Zero),
						IsLightDependent = true
					});*/

					_selectedModel.Materials[i] = clonedMaterial;
					//entity.Remove<ModelComponent>();
					//entity.Add(_model);
					/*.Passes.First().
				var newMaterial = (Material)_model.GetMaterial(i).MemberwiseClone();
				//newMaterial.Parameters = new ParameterCollection(material.Parameters);
				newMaterial.Passes = new MaterialPassCollection(_model.GetMaterial(i).Passes);

				var clonedMaterial = Material.New(GraphicsDevice, new MaterialDescriptor()
				{
					Attributes =
					{
						Diffuse = new MaterialDiffuseMapFeature(new ComputeColor(Color.White)),
						DiffuseModel = new MaterialDiffuseLambertModelFeature()
					}
				});
				_model.Materials[i] = clonedMaterial;*/
				}

				//_model.RenderGroup = RenderGroup.Group10;
				//_model.Materials.


			}
			/*foreach(Entity e in entity.GetChildren()) {
				var mod = e.Get<ModelComponent>();
			}*/
			//new MaterialInstance()
			//_model.Model.Materials.First().




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