using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Components.Runtime.Components
{
    public class WindowResizer : ImageComponent, IDragHandler 
    {
        private WindowComponent _window;

        public WindowResizer Build(WindowComponent window)
        {
            _window = window;
            return this;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            var size = _window.GetRect().sizeDelta;
            float newWidth = size.x + eventData.delta.x;
            float newHeight = size.y - eventData.delta.y;
            
            //Debug.Log($"{newWidth}x{newHeight} + {size}");

            Vector2 newSize = new Vector2(Mathf.Min(GUIService.GetCanvasWidth(), Mathf.Max(newWidth, _window.GetMinimumWindowSize().x)), Mathf.Min(GUIService.GetCanvasHeight(), Mathf.Max(newHeight, _window.GetMinimumWindowSize().y)));
            _window.Size(newSize).Render();
        }
    }
}