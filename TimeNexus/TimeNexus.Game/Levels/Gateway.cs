using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.ExtensionMethods;
using TimeNexus.Levels.LabyrinthLevelManager;

namespace TimeNexus.Levels
{
	public class Gateway : ScriptComponent
	{
		public Vector3 Direction { get; set; } = Vector3.UnitX;

		/// <summary>
		/// Is the gateway an exit?
		/// </summary>
		public bool IsExit { get; set; }

		/// <summary>
		/// The scene that comes after the gateway
		/// </summary>
		public Scene AttachedScene { get; private set; }

		/// <summary>
		/// The gateway of the attached scene
		/// </summary>
		public Gateway AttachedGateway { get; private set; }

		/// <summary>
		/// Attaches a scene to this gateway
		/// Will magically take care of everything.
		/// </summary>
		/// <param name="url">URL of the scene asset</param>
		/// <param name="attachedGatewayChooser">Which gateway(s) should it attach current scene to</param>
		/// <returns></returns>
		public async Task AttachScene(string url, Func<Gateway, bool> attachedGatewayChooser = null)
		{
			Level other = await LevelManager.Instance.LoadLevel(url);
			
			if(other.Gateways.Count == 0)
			{
				throw new ArgumentException("You can't attach a scene with 0 gateways.");
			}

			AttachedScene = other.Scene;

			if (attachedGatewayChooser == null) attachedGatewayChooser = (gateway) => !gateway.IsExit;
			AttachedGateway = other.Gateways
								.Where(attachedGatewayChooser) //Find then first non-exit gateway
								.DefaultIfEmpty(other.Gateways.First()) //Or just take the first gateway
								.First(); //Take the first element


			//Position the scene
			var thisTransform = this.Entity.Transform;
			var attachedTransform = AttachedGateway.Entity.Transform;
			
			//diff * otherRot = desiredRot
			//diff = desiredRot * inverse(otherRot)
			var desiredRotation = thisTransform.GetWorldRotation();
			desiredRotation.Invert();
			var otherRotation = attachedTransform.GetWorldRotation();

			otherRotation.Invert();
			//Rotate the scene

			other.Rotate(desiredRotation * otherRotation);

			//
			attachedTransform.UpdateWorldMatrix();
			//Calculate the position of the attached scene & move it
			AttachedScene.Offset = thisTransform.GetWorldPosition() - (attachedTransform.GetWorldPosition() - AttachedScene.Offset);


			//Place the level
			Entity.Scene.Children.Add(other.Scene);
			other.Dimension = LevelManager.Instance.GetFreeDimension(other);
			if(other.Dimension != LevelManager.DefaultDimension)
			{
				//TODO
			}
		}
	}
}
