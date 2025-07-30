using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Components.Runtime.Components
{
    public class WindowResizer : ImageComponent, IDragHandler 
    {
        private ResizableWindowComponent _resizableWindow;

        public override void Start()
        {
            base.Start();
            DisplayName = "WindowResizer";
        }

        public WindowResizer Build(ResizableWindowComponent resizableWindow)
        {
            _resizableWindow = resizableWindow;
            return this;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            var size = _resizableWindow.GetRect().sizeDelta;
            float newWidth = size.x + eventData.delta.x;
            float newHeight = size.y - eventData.delta.y;
            
            //Debug.Log($"{newWidth}x{newHeight} + {size}");

            Vector2 newSize = new Vector2(Mathf.Min(GUIService.GetCanvasWidth(), Mathf.Max(newWidth, _resizableWindow.GetMinimumWindowSize().x)), Mathf.Min(GUIService.GetCanvasHeight(), Mathf.Max(newHeight, _resizableWindow.GetMinimumWindowSize().y)));
            _resizableWindow.Size(newSize).Render();
        }
    }
}