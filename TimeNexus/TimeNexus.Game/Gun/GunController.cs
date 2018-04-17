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
using SiliconStudio.Core;
using TimeNexus.Player;

namespace TimeNexus.Gun
{
	public class GunController : SyncScript
	{
		[CanBeNull]
		public GunBeam GunBeam { get; set; }

		private ModelComponent _selectedModel;

		public Material EffectMaterial { get; set; }
		private MaterialPass effectRenderPass;

		public override void Start()
		{
			//Input.AddListener(new Test());
			effectRenderPass = EffectMaterial?.Passes?.First();

			PlayerRaycaster.OnRaycast.Subscribe(hitResult =>
			{
				var entity = hitResult.Collider?.Entity;
				if(entity == null)
				{
					GunBeam?.UpdateBeam(false);
					return;
				}

				if (Input.MouseWheelDelta != 0)
				{
					var timeComponent = entity.Get<TimeControllerComponent>() ?? entity.GetParent()?.Get<TimeControllerComponent>();
					if (timeComponent != null)
					{
						GunBeam?.UpdateBeam(true, hitResult.Point);
						if (Input.MouseWheelDelta < 0) timeComponent.Time = timeComponent.Time.GetPrevious();
						else timeComponent.Time = timeComponent.Time.GetNext();
					}
				}
				else
				{
					GunBeam?.UpdateBeam(false);
				}
			})
			.DisposeLater(this);
		}



		public override void Update()
		{
			return;

			Entity entity = null;
			var newModel = entity.Get<ModelComponent>();

			//if (_selectedModel != newModel) _selectedModel?.Materials
			if (newModel != null && _selectedModel != newModel && EffectMaterial != null)
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

		/*public class Test : IInputEventListener<KeyEvent>
		{
			public void ProcessEvent(KeyEvent inputEvent)
			{
				throw new NotImplementedException();
			}
		}*/

		private Material CloneMaterial(Material material)
		{
			var clone = new Material();
			foreach (var pass in material.Passes)
			{
				clone.Passes.Add(new MaterialPass(new ParameterCollection(pass.Parameters))
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