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
using TimeNexus.ExtensionMethods;
using TimeNexus.Effects;
using TimeNexus.Gun;
using SiliconStudio.Core.Annotations;

namespace TimeNexus.Gun
{
	public class GunController : SyncScript
	{
		// Declared public member fields and properties will show in the game studio
		private Simulation simulation;
		public CameraComponent Camera { get; set; }

		[CanBeNull]
		public GunBeam GunBeam { get; set; }

		private ModelComponent _selectedModel;

		public Material effectMaterial { get; set; }
		private MaterialPass effectRenderPass;

		public override void Start()
		{
			simulation = this.GetSimulation();
			effectRenderPass = effectMaterial?.Passes?.First();
			
			if (Camera == null) Log.Error("No camera attached to the gun script");
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
			var result = Raycast(Camera);
			if (!result.Succeeded || result.Collider.Entity == null)
			{
				_selectedModel?.Materials.Clear();
				return;
			}


			var entity = result.Collider.Entity;

			if (Input.MouseWheelDelta != 0)
			{
				var timeComponent = entity.Get<TimeControllerComponent>() ?? entity.GetParent()?.Get<TimeControllerComponent>();
				if (timeComponent != null)
				{
					GunBeam?.UpdateBeam(true, result.Point);
					if (Input.MouseWheelDelta < 0) timeComponent.Time = timeComponent.Time.GetPrevious();
					else timeComponent.Time = timeComponent.Time.GetNext();
				}
			}
			else
			{
				GunBeam?.UpdateBeam(false);
			}

			return;

			var newModel = entity.Get<ModelComponent>();

			//if (_selectedModel != newModel) _selectedModel?.Materials
			if (newModel != null && _selectedModel != newModel && effectMaterial != null)
			{
				_selectedModel = newModel;

				for (int i = 0; i < _selectedModel.GetMaterialCount(); i++)
				{
					//Remove the effect render pass from that other material
					//TODO: This will lead to issues when the model has multiple materials
					effectRenderPass.Material.Passes.Remove(effectRenderPass);

					//Clone the material
					var clonedMaterial = CloneMaterial(_selectedModel.GetMaterial(i));
					//Layers ~= render passes
					//Add the effect render pass to the material
					clonedMaterial.Passes.Add(
						effectRenderPass
					  );
					/* 
					var clonedPass1 = new MaterialPass(effectMaterial.Passes[0].Parameters); 
					clonedPass1.HasTransparency = pass.HasTransparency; 
					clonedPass1.BlendState = pass.BlendState; 
					clonedPass1.IsLightDependent = pass.IsLightDependent; 

					clone.Passes.Add(clonedPass1); 
					*/

					/*var parCol = new ParameterCollection();
					//parCol.Set(MaterialKeys.PixelStageSurfaceShaders, (SiliconStudio.Xenko.Shaders.ShaderClassSource)"hi");
					parCol.Set(MaterialKeys.DiffuseValue, Color4.White);
					clonedMaterial.Passes.Add(new MaterialPass(parCol)
						{
						HasTransparency = true,
						BlendState = new BlendStateDescription(Blend.One, Blend.Zero),
						IsLightDependent = true
					});*/

					//Override the default materials 
					_selectedModel.Materials[i] = clonedMaterial;
				}

			}
		}



		private Material CloneMaterial(Material material)
		{
			var clone = new Material();
			foreach (var pass in material.Passes)
			{
				clone.Passes.Add(new MaterialPass(pass.Parameters)
				{
					HasTransparency = pass.HasTransparency,
					BlendState = pass.BlendState,
					CullMode = pass.CullMode,
					IsLightDependent = pass.IsLightDependent
				});
			}
			return clone;
		}
	}
}