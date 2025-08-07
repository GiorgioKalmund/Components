using System;
using UnityEngine;

namespace Components.Runtime.Components.Game
{
    public class HotbarSlot : ButtonComponent, IFocusable, ICopyable<HotbarSlot>
    {
        public override void HandleFocus()
        {
            base.HandleFocus();
            Sprite("player", "Inventory Slot Selected");
        }

        public override void HandleUnfocus()
        {
            base.HandleUnfocus();
            Sprite("player", "Inventory Slot");
        }

        public int GetFocusGroup()
        {
            return 1;
        }

        public new HotbarSlot Copy(bool fullyCopyRect = true)
        {
            HotbarSlot copySlot = this.BaseCopy(this);
            return copySlot.CopyFrom(this, fullyCopyRect);
        }

        public HotbarSlot CopyFrom(HotbarSlot other, bool fullyCopyRect = true)
        {
            base.CopyFrom(other, fullyCopyRect);
            return this;
        }

        private void OnDestroy()
        {
            this.UnFocus();
        }
    }
}