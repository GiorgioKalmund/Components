using System;
using UnityEngine;

namespace Components.Runtime.Components.Game
{
    public class HotbarSlot : ImageComponent, IFocusable, ICopyable<HotbarSlot>
    {
        public void HandleFocus()
        {
            Sprite("player", "Inventory Slot Selected");
        }

        public void HandleUnfocus()
        {
            Sprite("player", "Inventory Slot");
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