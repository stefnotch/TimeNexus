using SiliconStudio.Core;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Games;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Xenko.UI.Controls;
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

		public UIComponent PauseUI { get; set; }

		private void Pause(bool pause)
		{
			if (Game.State != GameState.Running) return;

			if (pause)
			{
				Game.PlayTime.Pause();
				Simulation.DisableSimulation = true;
				Input.UnlockMousePosition();
				Game.IsMouseVisible = true;
				//Display da UI!
				PauseUI.Enabled = true;
			}
			else
			{
				Game.PlayTime.Resume();
				Simulation.DisableSimulation = false;
				Input.LockMousePosition(true);
				Game.IsMouseVisible = false;
				PauseUI.Enabled = false;
			}

		}
		
		public override void Start()
		{
			if (Instance != null) Log.Warning("Multiple GamePausers exist:" + Instance + "\n\n" + this);
			Instance = this;

			Button butt = (Button)(PauseUI.Page.RootElement.FindName("testButt"));
			butt.Click += Butt_Click;
		}

		private void Butt_Click(object sender, SiliconStudio.Xenko.UI.Events.RoutedEventArgs e)
		{
			throw new NotImplementedException();
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
