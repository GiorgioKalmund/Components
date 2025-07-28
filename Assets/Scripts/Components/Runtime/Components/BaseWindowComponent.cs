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
                _activeWindow?.Unfocus();
                _activeWindow = value;
                _activeWindow?.Focus();
            }
        }
        
        // Mask
        private RectMask2D _rectMask2D;
        
        // -- Subcomponents -- // 
        protected ImageComponent Header;
        protected ImageComponent Content;
        
        public override void Awake()
        {
            base.Awake();
            this.Alpha(0.4f).Color(UnityEngine.Color.black, true);
            ActiveWindow = this;
            this.Pivot(PivotPosition.UpperLeft, true);
            
            Header = ComponentBuilder.N<ImageComponent>(transform, "Header")
                    .Pivot(PivotPosition.UpperLeft, true)
                    .Alpha(0.4f)
                    .Color(UnityEngine.Color.black, true)
                ;
            
            Content = ComponentBuilder.N<ImageComponent>(transform, "Content")
                    .Pivot(PivotPosition.LowerCenter, true)
                    .Alpha(0.0f)
                    .Color(UnityEngine.Color.white, true)
                ;
            
            _rectMask2D = Content.gameObject.GetOrAddComponent<RectMask2D>();
        }
        
        private void Start()
        {
            Render();
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

        public virtual void Focus()
        {
            this.BringToFront();
            Alpha(1f);
        }

        public virtual void Unfocus()
        {
            Alpha(0.3f);
        }

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
        }
        public virtual void RenderContent()
        {
            Content.Size(this.GetWidth() - 2 * _contentPadding, this.GetHeight() - HeaderHeight - 2 * _contentPadding).Pos(0, _contentPadding);
        }
    }
}