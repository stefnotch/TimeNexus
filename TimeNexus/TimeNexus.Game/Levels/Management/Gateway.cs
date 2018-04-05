using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.ExtensionMethods;
using SiliconStudio.Core;

namespace TimeNexus.Levels.Management
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
		[DataMemberIgnore]
		public Level AttachedLevel { get; private set; }

		/// <summary>
		/// The gateway of the attached scene
		/// </summary>
		[DataMemberIgnore]
		public Gateway AttachedGateway { get; private set; }
		
		/// <summary>
		/// Attaches a scene to this gateway
		/// Will magically take care of everything.
		/// </summary>
		/// <param name="url">URL of the scene asset</param>
		/// <param name="attachedGatewayChooser">Which gateway(s) should it attach current scene to</param>
		/// <returns></returns>
		public async Task AttachLevel(string url, GatewayChooser gatewayChooser = null)
		{
			Level other = await LevelManager.Instance.LoadLevel(url);
			
			if(other.Gateways.Count == 0)
			{
				throw new ArgumentException("You can't attach a scene with 0 gateways.");
			}

			AttachedLevel = other;

			if (gatewayChooser == null) gatewayChooser = GatewayChoosers.NonExit;
			AttachedGateway = gatewayChooser(other);
			
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

			AttachedLevel.Rotate(desiredRotation * otherRotation);

			//
			attachedTransform.UpdateWorldMatrix();
			//Calculate the position of the attached scene & move it
			AttachedLevel.Scene.Offset = thisTransform.GetWorldPosition() - (attachedTransform.GetWorldPosition() - AttachedLevel.Scene.Offset);


			//Place the level
			Entity.Scene.Children.Add(AttachedLevel.Scene);
			AttachedLevel.Dimension = LevelManager.Instance.GetFreeDimension(AttachedLevel);
			if(AttachedLevel.Dimension != LevelManager.DefaultDimension)
			{
				//TODO
			}
		}
	}
}
