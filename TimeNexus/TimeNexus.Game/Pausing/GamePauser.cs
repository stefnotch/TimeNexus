using SiliconStudio.Core;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Games;
using SiliconStudio.Xenko.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeNexus.Input;

namespace TimeNexus.Pausing
{
	public class GamePauser : SyncScript
	{
		[DataMemberIgnore]
		public GamePauser Instance { get; private set; }
		
		[DataMemberIgnore]
		private bool _isPaused = false;

		[DataMemberIgnore]
		public bool IsPaused
		{
			get => _isPaused;
			set
			{
				_isPaused = value;
				Pause(_isPaused);
			}
		}

		private void Pause(bool pause)
		{
			if (Game.State != GameState.Running) return;

			if (pause)
			{
				Game.PlayTime.Pause();
				Simulation.DisableSimulation = true;
				//this.Input.Enabled = false;
				this.Input.PreUpdateInput += PreUpdateInput;
			}
			else
			{
				Game.PlayTime.Resume();
				Simulation.DisableSimulation = false;
				//this.Input.Enabled = true;
			}

		}

		private void PreUpdateInput(object sender, SiliconStudio.Xenko.Input.InputPreUpdateEventArgs e)
		{
			this.Input.Enabled = false;
			//Input.ProcessEvent
		}

		public override void Start()
		{
			if (Instance != null) Log.Warning("Multiple GamePausers exist:" + Instance + "\n\n" + this);
			Instance = this;
		}

		public override void Update()
		{
			if(Input.IsKeyReleased(KeyBindings.Pause))
			{
				IsPaused = !IsPaused;
			}
		}
	}
}
