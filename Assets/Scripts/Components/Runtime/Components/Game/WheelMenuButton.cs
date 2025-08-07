using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Components.Runtime.Components.Game
{
    public class WheelMenuButton : ButtonComponent
    {
        private Sprite _normalSprite;
        private Sprite _highlightedSprite;
        public override void Start()
        {
            base.Start();

            ForegroundImage.Rotation(0);
            ButtonText.Rotation(0);
            ForegroundImage.LocalScale(0.5f, 0.5f);
        }

        public override void HandlePointerEnter(PointerEventData eventData)
        {
            base.HandlePointerEnter(eventData);
            this.Focus();
        }

        public override void HandlePointerExit(PointerEventData eventData)
        {
            base.HandlePointerEnter(eventData);
            this.UnFocus();
        }

        public WheelMenuButton ConfigureSprites(Sprite normal, Sprite highlighted)
        {
            _normalSprite = normal;
            _highlightedSprite = highlighted;
            
            Sprite(_normalSprite);
            return this;
        }

        public override void HandleFocus()
        {
            base.HandleFocus();
            Sprite("player", "Menu Wheel Slot Selected");
        }


        public override void HandleUnfocus()
        {
            base.HandleUnfocus();
            Sprite("player", "Menu Wheel Slot");
        }
        
        private void OnDrawGizmos()
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = UnityEngine.Color.red;
            style.fontSize = 14; 
            Handles.Label(transform.position, GetFocusGroup() + "", style);
        }
    }
}