using UnityEngine;
using UnityEngine.InputSystem;

namespace Components.Runtime.Components
{
    public class ResizableWindowComponent : BaseWindowComponent
    {
        
        // -- Subcomponents -- //
        protected WindowResizer WindowResizer;

        protected Vector2 MinimumWindowSize = new Vector2(100, 100);
        private bool _allowResize = true;

        public override void Awake()
        {
            base.Awake();

            WindowResizer = ComponentBuilder.N<WindowResizer>(WindowBase)
                .Pivot(PivotPosition.LowerRight, true)
                .Size(40, 40)
                .Build(this)
                .Sprite("gui_assets" ,"resizer")
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
            base.Build(action);
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
            WindowResizer.SetActive(!_allowResize);
        }
        
        public override void Expand()
        {
            base.Expand();
            WindowResizer.SetActive(_allowResize);
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

        public ResizableWindowComponent NoResize()
        {
            _allowResize = false;
            WindowResizer.SetActive(false);
            return this;
        }
    }
}