using System;
using System.Collections;
using Components.Runtime.Service;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Components.Runtime.Components
{
    [Flags]
    public enum ScrollViewDirection
    {
        Horizontal = 1,
        Vertical = 2,
        Both = Horizontal | Vertical,
        None = 0
        
    }
    
    public class ScrollViewComponent : ImageComponent , IPointerEnterHandler, IPointerExitHandler
    {
        public ImageComponent content;

        private RectMask2D _mask;
        private ContentSizeFitter _fitter;
        private ScrollRect _scroll;

        private VerticalLayoutGroup _vStack;
        private GridLayoutGroup _grid;

        public override void Awake()
        {
            base.Awake();

            _scroll = gameObject.AddComponent<ScrollRect>();
            _mask = gameObject.AddComponent<RectMask2D>();

            content = ComponentBuilder.N<ImageComponent>("Content", transform)
                    .Alpha(0f)
                ;
            _fitter = content.gameObject.AddComponent<ContentSizeFitter>();

            _scroll.content = content.GetRect();

            _scroll.movementType = ScrollRect.MovementType.Clamped;
        }

        public override void Start()
        {
            base.Start();
            DisplayName = "ScrollViewComponent";
        }

        public ScrollViewComponent AddVerticalLayout(float spacing, TextAnchor childAlignment = TextAnchor.MiddleCenter, bool childControlWidth = false, bool childControlHeight = false, bool childForceExpandWidth = false, bool childForceExpandHeight = false, bool reverseArrangement = false) 
        {
            _vStack = AddLayout<VerticalLayoutGroup>(content.gameObject,spacing, childAlignment, childControlWidth, childControlHeight, childForceExpandWidth, childForceExpandHeight,reverseArrangement);
            return this;
        }

        public new ScrollViewComponent ContentPadding(PaddingSide side, int amount)
        {
            base.ContentPadding(side, amount);
            if (side.HasFlag(PaddingSide.Leading)) { if (_vStack) _vStack.padding.left = amount; }
            if (side.HasFlag(PaddingSide.Trailing)) { if (_vStack) _vStack.padding.right = amount; }
            if (side.HasFlag(PaddingSide.Top)) { if (_vStack) _vStack.padding.top = amount; }
            if (side.HasFlag(PaddingSide.Bottom)) { if (_vStack) _vStack.padding.bottom = amount; }
            
            return this;
        }

        public void ScrollToBottom()
        {
            content.Pos(content.GetPos().x, (content.GetHeight() -  this.GetHeight()) / 2);
        }
        
        public void ScrollToTop()
        {
            content.Pos(content.GetPos().x, -(content.GetHeight() -  this.GetHeight()) / 2);
        }

        public ScrollViewComponent AddContent(BaseComponent component)
        {
            component.Parent(content);

            // Maybe there is a more elegant, less computationally heavy solution, but this works for now
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetRect());
            _vStack.CalculateLayoutInputVertical();
            _vStack.SetLayoutVertical();
            
            return this;
        }

        public override BaseComponent HandleSizeChanged(float x, float y)
        {
            base.HandleSizeChanged(x, y);
            if (content)
                content.Size(x, y);
            return this;
        }
        
        public ScrollViewComponent ScrollingDirection(ScrollViewDirection dir)
        {
            _scroll.vertical = dir.HasFlag(ScrollViewDirection.Vertical);
            _scroll.horizontal = dir.HasFlag(ScrollViewDirection.Horizontal);
            return this;
        }
        public ScrollViewComponent FittingDirection(ScrollViewDirection dir)
        {
            _fitter.verticalFit = dir.HasFlag(ScrollViewDirection.Vertical) ? ContentSizeFitter.FitMode.MinSize : ContentSizeFitter.FitMode.Unconstrained;
            _fitter.horizontalFit = dir.HasFlag(ScrollViewDirection.Horizontal) ? ContentSizeFitter.FitMode.MinSize : ContentSizeFitter.FitMode.Unconstrained;
            return this;
        }

        public ScrollViewComponent MovementType(ScrollRect.MovementType movementType)
        {
            _scroll.movementType = movementType;
            return this;
        }

        // -- Idea: Disable certain controls when hovering over scroll views, to avoid scrolling in other areas as well -- //
        public override void HandlePointerEnter(PointerEventData eventData)
        {
            InputService.Input.UI.ScrollWheel.Disable();
        }

        public override void HandlePointerExit(PointerEventData eventData)
        {
            InputService.Input.UI.ScrollWheel.Enable();
        }

        public VerticalLayoutGroup GetVerticalLayout()
        {
            return _vStack;
        }

        public ContentSizeFitter GetFitter()
        {
            return _fitter;
        }
    }
}