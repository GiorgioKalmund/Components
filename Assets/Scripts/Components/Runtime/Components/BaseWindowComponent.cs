using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Components.Runtime.Components
{
    public class BaseWindowComponent : ImageComponent, IPointerDownHandler, IFocusable, IDragHandler, IRenderable
    {
        // -- Canvas sizes stored to avoid duplicate calculations -- //
        protected static float CanvasWidth;
        protected static float CanvasHeight;
        protected float HeaderHeight = 30f;
        // -- Border offset to allow for margins around the screen -- //
        private float _borderOffset = 0f;
        private float _contentPadding = 0f;

        // -- Focus State for ActiveWindow handling -- //
        private static BaseWindowComponent _activeWindow;
        public static BaseWindowComponent ActiveWindow
        {
            get => _activeWindow;
            set
            {
                _activeWindow?.HandleUnfocus();
                _activeWindow = value;
                _activeWindow?.HandleFocus();
            }
        }

        public bool Focused => ActiveWindow == this;
        
        // Mask
        private RectMask2D _rectMask2D;
        
        // -- Subcomponents -- // 
        private ButtonComponent _minimizeMaximizeButton;
        protected ImageComponent Header;
        protected ImageComponent Content;
        
        // -- Dragging -- // 
        protected bool AllowDragging = true;
        
        // -- Minimize / Maximize -- //
        private Vector2 _maximizedSize;
        
        public override void Awake()
        {
            base.Awake();
            this.Pivot(PivotPosition.UpperLeft, true);
            
            Header = ComponentBuilder.N<ImageComponent>(transform, "Header")
                    .Pivot(PivotPosition.UpperLeft, true)
                    .Color(UnityEngine.Color.gray3, true)
                ;

            _minimizeMaximizeButton = ComponentBuilder.N<ButtonComponent>(Header, "MinimizeMaximize")
                .Pivot(PivotPosition.MiddleRight, true)
                .Create("-", ToggleMinimizeMaximize)
                ;
            
            Content = ComponentBuilder.N<ImageComponent>(transform, "Content")
                    .Pivot(PivotPosition.LowerCenter, true)
                    .Alpha(0.0f)
                ;
            
            _rectMask2D = Content.gameObject.GetOrAddComponent<RectMask2D>();
            
            ActiveWindow = this;
        }
        
        private void Start()
        {
            Render();
        }

        public void ToggleMinimizeMaximize()
        {
            // If the window isn't focus, we first focus it and on a second click, we act accordingly
            if (!Focused)
            {
                ActiveWindow = this;
                return;
            }
            if (_maximizedSize.magnitude > 0)
                Maximize();
            else 
                Minimize();
        }

        protected virtual void Minimize()
        {
            _maximizedSize = GetRect().sizeDelta;
            this.Height(HeaderHeight);
            Content.SetActive(false);
            _minimizeMaximizeButton.Text("+");
        }
        protected virtual void Maximize()
        {
            if (_maximizedSize.magnitude > 0)
                this.Height(_maximizedSize.y);
            _maximizedSize = Vector2.zero;
            Content.SetActive(true);
            _minimizeMaximizeButton.Text("-");
        }

        public BaseWindowComponent RecalculateSizes()
        {
            CanvasWidth = GUIService.GetCanvasWidth();
            CanvasHeight= GUIService.GetCanvasHeight();
            return this;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            this.BringToFront();
            ActiveWindow = this;
            RecalculateSizes();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!AllowDragging)
                return;
            
            var pos = GetRect().anchoredPosition;
            Vector2 wantToBePos = (pos + eventData.delta);
            Vector2 willBePos = pos;

            if (wantToBePos.x > _borderOffset && wantToBePos.x < CanvasWidth - this.GetWidth() - _borderOffset)
            {
                willBePos.x = wantToBePos.x;
            }
            if (wantToBePos.y < _borderOffset && wantToBePos.y > -CanvasHeight + HeaderHeight + _borderOffset)
            {
                willBePos.y = wantToBePos.y;
            }

            GetRect().anchoredPosition = willBePos;
        }

        public virtual void HandleFocus() { }

        public virtual void HandleUnfocus() { }

        public BaseWindowComponent ContentPadding(float padding)
        {
            _contentPadding = padding;
            return this;
        }

        public BaseWindowComponent AddContent(BaseComponent component)
        {
            component.Parent(Content);
            return this;
        }
        
        public virtual void Render()
        {
            RenderHeader();
            RenderContent();
        }
        
        public virtual void RenderHeader()
        {
            Header.Size(this.GetWidth(), HeaderHeight);
            _minimizeMaximizeButton.Size(HeaderHeight, HeaderHeight);
        }
        public virtual void RenderContent()
        {
            Content.Size(this.GetWidth() - 2 * _contentPadding, this.GetHeight() - HeaderHeight - 2 * _contentPadding).Pos(0, _contentPadding);
        }
    }
}