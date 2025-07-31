using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Components.Runtime.Components
{
    public class ResizableWindowComponent : BaseWindowComponent
    {
        
        // -- Subcomponents -- //
        private WindowResizer _windowResizer;


        protected Vector2 MinimumWindowSize = new Vector2(100, 100);

        public override void Awake()
        {
            base.Awake();

            _windowResizer = ComponentBuilder.N<WindowResizer>(WindowBase)
                .Pivot(PivotPosition.LowerRight, true)
                .Size(30, 30)
                .Build(this)
                .Sprite("resizer")
                .Cast<WindowResizer>()
                ;
        }

        public override void Start()
        {
            base.Start();
            DisplayName = "ResizableWindowComponent";
        }

        
        public ResizableWindowComponent Build(InputAction action, string title)
        {
            Build(action);
            Title = title;
            return this;
        }
        
        public ResizableWindowComponent Build(string title)
        {
            return Build(null, title);
        }
        
        public ResizableWindowComponent Build(InputAction action, string title, Color windowColor, Color headerColor)
        {
            Build(action, title);
            WindowBase.Color(windowColor);
            Header.Color(headerColor);
            return this;
        }
        
        public ResizableWindowComponent Build(string title, Color windowColor, Color headerColor)
        {
            return Build(null ,title, windowColor, headerColor);
        }

        public override void Collapse()
        {
            base.Collapse();
            _windowResizer.SetActive(false);
        }
        
        public override void Expand()
        {
            base.Expand();
            _windowResizer.SetActive(true);
        }

        public ResizableWindowComponent MinimumSize(Vector2 minSize)
        {
            MinimumWindowSize = minSize;
            return this;
        }

        public ResizableWindowComponent MinimumSize(float x, float y)
        {
            return MinimumSize(new Vector2(x, y));
        }

        public Vector2 GetMinimumWindowSize()
        {
            return MinimumWindowSize;
        }
    }
}