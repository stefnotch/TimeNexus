using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Physics;
using SiliconStudio.Core;
using static SiliconStudio.Xenko.Audio.AudioLayer;
using SiliconStudio.Core.Collections;
using System.Windows.Input;

namespace ThirdPersonPlatformer
{
    public class Pause : SyncScript
    {
        private TimeSpan last_pause;
        private Keys pause_key = Keys.Escape;
        private bool is_paused;
        public UIComponent PauseMenu;
        //private List<IPausable> Pausables { get; } = new List<IPausable>();

        public bool IsPaused
        {
            get => this.is_paused;
            private set => this.is_paused = value;
        }

        public override void Start()
        {
            this.PauseGame(false);
        }

        public override void Update()
        {
            this.SetPaused();
        }

        private void SetPaused()
        {
            if (this.IsPaused)
            {
                this.Input.Enabled = true;
                if (this.Input.IsKeyReleased(this.pause_key)) this.PauseGame(false);
                else this.Input.Enabled = false;
                return;
            }
            else
            {
                if (!Input.IsKeyReleased(this.pause_key) || (float)((this.Game.PlayTime.TotalTimeWithPause - this.last_pause).Milliseconds) < 500f) return;
                this.PauseGame(!this.IsPaused);
                this.last_pause = this.Game.PlayTime.TotalTimeWithPause;
            }
        }

        private TrackingCollection<IInputSource> sources;
        private void PauseGame(bool val)
        {
            Game.IsMouseVisible = val;
            this.Input.Enabled = !(Simulation.DisableSimulation = this.PauseMenu.Enabled = this.IsPaused = val);

            if (val)
            {
                sources = this.Input.Sources;
                //this.Input.ResetSources();
            }
            else
            {
                this.Input.LockMousePosition(true);
                //this.Input.Sources.AddRange(sources);
            }
        }

        //private bool PausePausables(bool val)
        //{
        //    foreach (IPausable p in this.Pausables) p.Pause(val);
        //    return val;
        //}
    }
}