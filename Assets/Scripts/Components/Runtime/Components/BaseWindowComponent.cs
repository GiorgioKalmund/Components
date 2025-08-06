using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using ColorUtility = Unity.VisualScripting.ColorUtility;

namespace Components.Runtime.Components
{
    public class BaseWindowComponent : BaseComponent, IPointerDownHandler, IFocusable, IBeginDragHandler, IDragHandler, IRenderable
    {
        // -- Canvas sizes stored to avoid duplicate calculations -- //
        protected static float CanvasWidth;
        protected static float CanvasHeight;
        protected float HeaderHeight = 30f;
        protected float MinimumWindowShowingHeight = 30f;
        protected float HeaderToolsWidth = 30f;
        // -- Border offset to allow for margins around the screen -- //
        private float _borderOffset = 0f;
        private float _contentPadding = 0f;

        // Key for showing / hiding the window, initial state -- //
        private InputAction _toggleInputAction;
        private bool _startOfHidden;
        public bool Hidden => !WindowBase?.gameObject.activeSelf ?? true;
        
        // Mask
        private RectMask2D _rectMask2D;
        
        // -- Subcomponents -- //
        protected ImageComponent WindowBase;
        private ButtonComponent _minimizeMaximizeButton;
        protected ImageComponent Header;
        protected ScrollViewComponent ScrollContent;
        protected ImageComponent HeaderTools;
        private TextComponent _headerText;
        protected string Title = "";
        
        // -- Dragging -- // 
        protected bool AllowDragging = true;
        
        // -- Minimize / Maximize -- //
        private Vector2 _maximizedSize;

        public override void Awake()
        {
            base.Awake();
            this.Pivot(PivotPosition.UpperLeft, true);

            // ===== WINDOW BASE-- Parent of all subcomponents to allow hiding == //
            WindowBase = ComponentBuilder.N<ImageComponent>(transform, "Window Base");
            // ==================================================================== //
            
            Header = ComponentBuilder.N<ImageComponent>(WindowBase, "Header")
                    .Pivot(PivotPosition.UpperLeft, true)
                    .Color(UnityEngine.Color.gray3, true)
                ;

            HeaderTools = ComponentBuilder.N<ImageComponent>(Header, "Header Tools")
                    .Pivot(PivotPosition.MiddleLeft, true)
                    .Alpha(0)
                ;
            
            _headerText = ComponentBuilder.N<TextComponent>(HeaderTools)
                    .Pivot(PivotPosition.MiddleLeft)
                    .AnchoredTo(PivotPosition.MiddleRight)
                    .FontSize(HeaderHeight - 10)
                    .VAlignCenter()
                    .OverflowMode(TextOverflowModes.Ellipsis)
                ;

            _minimizeMaximizeButton = ComponentBuilder.N<ButtonComponent>(HeaderTools, "MinimizeMaximize")
                .Pivot(PivotPosition.MiddleLeft, true)
                .Create(action:() => ToggleCollapse())
                .Color(UnityEngine.Color.gray8)
                .Alpha(0.7f)
                .Cast<ButtonComponent>()
                ;
            
            ScrollContent = ComponentBuilder.N<ScrollViewComponent>(WindowBase, "Content").Pivot(PivotPosition.LowerCenter, true);
            
            _rectMask2D = ScrollContent.gameObject.GetOrAddComponent<RectMask2D>();
        }

        public ScrollViewComponent ConfigureContent()
        {
            return ScrollContent;
        }

        public virtual BaseWindowComponent Build(InputAction action)
        {
            _toggleInputAction = action;
            _toggleInputAction.Enable();
            return this;
        }
        
        private void OnEnable()
        {
            _toggleInputAction?.Enable();
        }

        public virtual void Start()
        {
            Render();
            Expand();
            DisplayName = "BaseWindowComponent";
            
            if (_startOfHidden)
                Minimize();
            
            if (_toggleInputAction != null)
                _toggleInputAction.performed += HandleOpenAndMinimize;
        }

        private void OnDisable()
        {
            _toggleInputAction?.Disable();
        }

        // To make support other subcomponent showing & hiding appropriately, please make sure to never parent to 'transform', but always to 'WindowBase'!
        private void HandleOpenAndMinimize(InputAction.CallbackContext callback)
        {
            if (Hidden)
                Open();
            else 
                Minimize();
        }
        public void Open()
        {
            WindowBase.SetActive(true);
            this.BringToFront();
        }
        public void Minimize()
        {
            WindowBase.SetActive(false);
        }

        public void ToggleCollapse(bool force = false)
        {
            this.Focus();
            if (_maximizedSize.magnitude > 0)
                Expand();
            else 
                Collapse();
        }

        public virtual void Collapse()
        {
            _maximizedSize = GetRect().sizeDelta;
            this.Height(HeaderHeight);
            ScrollContent.SetActive(false);
            _minimizeMaximizeButton.Foreground(ImageService.GetSpriteFromAsset("gui_assets", "right_arrow"));
        }
        public virtual void Expand()
        {
            if (_maximizedSize.magnitude > 0)
                this.Height(_maximizedSize.y);
            _maximizedSize = Vector2.zero;
            ScrollContent.SetActive(true);
            _minimizeMaximizeButton.Foreground(ImageService.GetSpriteFromAsset("gui_assets", "down_arrow"));
        }

        public BaseWindowComponent RecalculateSizes()
        {
            CanvasWidth = GUIService.GetCanvasWidth();
            CanvasHeight= GUIService.GetCanvasHeight();
            return this;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            this.Focus();
            RecalculateSizes();
        }
        
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            this.Focus();
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
            if (wantToBePos.y < _borderOffset && wantToBePos.y > -CanvasHeight + MinimumWindowShowingHeight + _borderOffset)
            {
                willBePos.y = wantToBePos.y;
            }

            GetRect().anchoredPosition = willBePos;
        }

        public virtual void HandleFocus()
        {
            this.BringToFront();
            Header.Alpha(1f);
        }

        public virtual void HandleUnfocus()
        {
            Header.Alpha(0.4f);
        }

        public BaseWindowComponent ContentPadding(float padding)
        {
            _contentPadding = padding;
            return this;
        }

        public BaseWindowComponent AddContent(BaseComponent component)
        {
            ScrollContent.AddContent(component);
            return this;
        }
        
        public BaseWindowComponent AddContent(params BaseComponent[] components)
        {
            foreach (var baseComponent in components)
            {
                AddContent(baseComponent);
            }
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
            HeaderTools.Size(HeaderToolsWidth, HeaderHeight);
            _headerText.Text(Title).Size(this.GetWidth() - HeaderHeight, HeaderHeight)
                .Padding(PaddingSide.Horizontal, 10);
            _minimizeMaximizeButton.Size(30f, HeaderHeight);
        }
        public virtual void RenderContent()
        {
        }

        public override BaseComponent HandleSizeChanged(float x, float y)
        {
            base.HandleSizeChanged(x, y);
            WindowBase?.Size(x, y);
            
            ScrollContent.Size(this.GetWidth() - 2 * _contentPadding, this.GetHeight() - HeaderHeight - 2 * _contentPadding).Pos(0, _contentPadding);
            if (!ScrollContent.contentHasBeenSizedManually)
            {
                Vector2 maxSize = ScrollContent.GetSize();
                ScrollContent.SizeContent(maxSize.x, maxSize.y);
            }
            return this;
        }

        public BaseWindowComponent StartHidden(bool hidden = true)
        {
            _startOfHidden = hidden;
            return this;
        }

        public BaseWindowComponent NoHeader()
        {
            HeaderHeight = 0;
            Header.SetActive(false);
            return this;
        }

        public BaseWindowComponent SetBase(Sprite sprite)
        {
            WindowBase.Sprite(sprite);
            return this;
        }
        
        public BaseWindowComponent SetBase(Color color)
        {
            WindowBase.Sprite((Sprite)null);
            WindowBase.Color(color);
            return this;
        }
        
        public BaseWindowComponent SetContent(Sprite sprite)
        {
            ScrollContent.Sprite(sprite);
            return this;
        }
        
        public BaseWindowComponent SetContent(Color color)
        {
            ScrollContent.ClearSprite();
            ScrollContent.Color(color);
            return this;
        }
        
        public BaseWindowComponent SetHeaderColor(Color color)
        {
            Header.Color(color);
            return this;
        }
    }
}