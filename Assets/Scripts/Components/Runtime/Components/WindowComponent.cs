using TMPro;
using UnityEngine;

namespace Components.Runtime.Components
{
    public class WindowComponent : BaseWindowComponent
    {
        protected string Title = "";
        
        // -- Subcomponents -- //
        private TextComponent _headerText;
        private WindowResizer _windowResizer;


        protected Vector2 MinimumWindowSize = new Vector2(100, 100);

        public override void Awake()
        {
            base.Awake();
            DisplayName = "WindowComponent";

            _headerText = ComponentBuilder.N<TextComponent>(Header)
                    .Pivot(PivotPosition.UpperLeft, true)
                    .FontSize(HeaderHeight - 5)
                    .VAlignCenter()
                    .OverflowMode(TextOverflowModes.Ellipsis)
                ;

            _windowResizer = ComponentBuilder.N<WindowResizer>(transform)
                .Pivot(PivotPosition.LowerRight, true)
                .Size(30, 30)
                .Build(this)
                ;
        }

        public WindowComponent Build(string title)
        {
            Title = title;
            return this;
        }

        public override void RenderHeader()
        {
            base.RenderHeader();
            _headerText.Text(Title).Size(this.GetWidth(), HeaderHeight);
        }

        public Vector2 GetMinimumWindowSize()
        {
            return MinimumWindowSize;
        }
    }
}