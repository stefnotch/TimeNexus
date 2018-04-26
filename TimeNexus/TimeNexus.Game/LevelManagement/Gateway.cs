using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core;
using SiliconStudio.Xenko.Rendering;
using SiliconStudio.Xenko.Engine.Design;

namespace TimeNexus.LevelManagement
{
	[DefaultEntityComponentProcessor(typeof(GameStudioGatewayProcessor), ExecutionMode = ExecutionMode.Editor)]
	public class Gateway : ScriptComponent
	{
		public Quaternion Rotation { get; set; } = Quaternion.Identity;

		/// <summary>
		/// Is the gateway an exit?
		/// </summary>
		public bool IsExit { get; set; }

		/// <summary>
		/// The level that comes after the gateway
		/// </summary>
		[DataMemberIgnore]
		public Level AttachedLevel { get; private set; }

		/// <summary>
		/// The gateway of the attached level
		/// </summary>
		[DataMemberIgnore]
		public Gateway AttachedGateway { get; private set; }

		[DataMemberIgnore]
		public bool IsLoading { get; private set; }

		/// <summary>
		/// Attaches a level to this gateway
		/// Will magically take care of everything.
		/// </summary>
		/// <param name="url">URL of the scene asset</param>
		/// <param name="gatewayChooser"></param>
		/// <param name="attachedGatewayChooser">Which gateway(s) should it attach current scene to</param>
		/// <returns></returns>
		public async Task AttachLevel(string url, GatewayChooser gatewayChooser = null)
		{
			if(AttachedLevel != null)
			{
				//TODO: Take care of this case
				Log.Error("already has an attached level: " + url);
			}
			IsLoading = true;
			Level other = await LevelManager.Instance.LoadLevel(url).ConfigureAwait(false);
			
			if(other.Gateways.Count == 0)
			{
				throw new ArgumentException("You can't attach a scene with 0 gateways. Offending scene: " + url);
			}

			AttachedLevel = other;

			if (gatewayChooser == null) gatewayChooser = GatewayChoosers.NonExit;
			AttachedGateway = gatewayChooser(AttachedLevel);
			
			//Position the scene
			var thisTransform = this.Entity.Transform;
			var attachedTransform = AttachedGateway.Entity.Transform;

			//diff * otherRot = desiredRot
			//diff = desiredRot * inverse(otherRot)
			thisTransform.GetWorldTransformation(out Vector3 thisPosition, out Quaternion desiredRotation, out _);
			desiredRotation = this.Rotation * desiredRotation;
			desiredRotation = Quaternion.RotationY(MathUtil.Pi) * desiredRotation;

			attachedTransform.UpdateWorldMatrix();
			attachedTransform.GetWorldTransformation(out _, out Quaternion otherRotation, out _);
			otherRotation = AttachedGateway.Rotation * otherRotation;
			otherRotation.Invert();

			//Rotate the scene
			AttachedLevel.Rotate(otherRotation * desiredRotation);


			attachedTransform.UpdateWorldMatrix();
			attachedTransform.GetWorldTransformation(out Vector3 otherPosition, out _, out _);
			//Calculate the position of the attached scene & move it
			AttachedLevel.Translate(thisPosition - (otherPosition - AttachedLevel.Scene.Offset));

			//Place the level
			if (!LevelManager.Instance.PlaceLevel(AttachedLevel))
			{
				Log.Error("Level dimension: " + AttachedLevel.Dimension + "");
				//TODO:
			}

			IsLoading = false;
		}
	}
}
