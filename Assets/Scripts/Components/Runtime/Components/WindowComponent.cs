using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

            _headerText = ComponentBuilder.N<TextComponent>(HeaderTools)
                    .Pivot(PivotPosition.MiddleLeft)
                    .AnchoredTo(PivotPosition.MiddleRight)
                    .FontSize(HeaderHeight - 5)
                    .VAlignCenter()
                    .OverflowMode(TextOverflowModes.Ellipsis)
                ;

            _windowResizer = ComponentBuilder.N<WindowResizer>(transform)
                .Pivot(PivotPosition.LowerRight, true)
                .Size(30, 30)
                .Build(this)
                .Sprite("resizer")
                .Cast<WindowResizer>()
                ;

            Content.Alpha(1).Sprite("backdrop_1").ImageType(Image.Type.Tiled).PixelsPerUnitMultiplier(0.33f);
            Color(UnityEngine.Color.gray6);
        }

        public override void Start()
        {
            base.Start();
            DisplayName = "WindowComponent";
        }

        public WindowComponent Build(string title)
        {
            Title = title;
            return this;
        }
        
        
        public WindowComponent Build(string title, Color windowColor, Color headerColor)
        {
            Build(title);
            Color(windowColor);
            Header.Color(headerColor);
            return this;
        }
        
        public override void HandleFocus()
        {
            base.HandleFocus();
            this.BringToFront();
            Header.Alpha(1f);
        }

        public override void HandleUnfocus()
        {
            Header.Alpha(0.4f);
        }

        protected override void Minimize()
        {
            base.Minimize();
            _windowResizer.SetActive(false);
        }
        
        protected override void Maximize()
        {
            base.Maximize();
            _windowResizer.SetActive(true);
        }

        public override void RenderHeader()
        {
            base.RenderHeader();
            _headerText.Text(Title).Size(this.GetWidth() - HeaderHeight, HeaderHeight)
                .Padding(PaddingSide.Horizontal, 10);
        }

        public Vector2 GetMinimumWindowSize()
        {
            return MinimumWindowSize;
        }
    }
}