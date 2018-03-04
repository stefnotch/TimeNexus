using SiliconStudio.Core.Annotations;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Games.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.Effects;
using TimeNexus.ExtensionMethods;

namespace TimeNexus.Gun
{
	public class GunBeam : StartupScript
	{
		private readonly TimeSpan FADE_TIME = TimeSpan.FromMilliseconds(300);

		private TimeSpan _lastFired = new TimeSpan(0);
		private Vector3 _target = Vector3.Zero;

		public Entity GunBeamEntity { get; set; }
		public ModelComponent GunBeamModel { get; set; }

		public void UpdateBeam(bool shoot, Vector3 target = default(Vector3))
		{
			if (GunBeamEntity == null || GunBeamModel == null) return;
			var currentTime = Game.PlayTime.TotalTime; //Game.UpdateTime.Total
													   //Fade the beam
			var fadeTime = currentTime.Subtract(_lastFired);
			if (!shoot && fadeTime > FADE_TIME)
			{
				GunBeamModel.Enabled = false;
				return;
			}

			if (shoot)
			{
				_target = target;
			}

			//Fire at the target


			//TODO: Do some interpolation stuff??
			Vector3 direction = _target - GunBeamEntity.Transform.GetWorldPosition();
			SetBeamLength(Math.Abs(direction.Length()));

			GunBeamEntity.Transform.LookAt(ref _target);	

			if (shoot)
			{
				GunBeamModel.Enabled = true;
				_lastFired = currentTime;
			}
		}

		private void SetBeamLength(float beamLength)
		{
			GunBeamModel.Entity.Transform.Scale.Y = beamLength;
			GunBeamModel.Entity.Transform.Position.Z = -beamLength / 2;

			GunBeamModel.GetMaterial(0).Passes[0].Parameters.Set(GunBeamKeys.BeamLength, beamLength);
		}

		public override void Start()
		{
			var logger = SiliconStudio.Core.Diagnostics.GlobalLogger.GetLogger("GunBeam");
			if (GunBeamEntity == null)
			{
				logger.Error("No gun beam container attached to the gun script");
			}

			if (GunBeamModel == null)
			{
				logger.Error("No gun beam model attached to the gun script");
			}
			else
			{
				GunBeamModel.Enabled = false;
			}

		}
	}
}
