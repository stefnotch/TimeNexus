using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Input;
using SiliconStudio.Xenko.Engine;
using TimeNexus.Objects;
using System.Reactive.Linq;
using TimeNexus.Player;
using TimeNexus.ExtensionMethods;

namespace Menu
{
    public class ButtonTrigger : SyncScript
    {
        private const int MAX_DIST = 10;
        public TriggerAction action;

        private static Vector3 GetPlayerPos
        {
            get => PlayerController.Player.Transform.GetWorldPosition();
        }

        public override void Start()
        {
            this.Entity.GetOrCreate<LookatTrigger>().OnLookatStart.Where(hitRes => (hitRes.Point - GetPlayerPos).Length() < MAX_DIST);
            
        }

        public override void Update()
        {

        }
    }

    public abstract class TriggerAction : ScriptComponent
    {
        public abstract void Action();
    }
}
