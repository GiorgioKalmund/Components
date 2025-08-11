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

        public bool contentHasBeenSizedManually = false; 
        private bool _sizeFitsContents;
        public bool SizeFitsContents
        {
            get => _sizeFitsContents;
            set
            {
                var oldValue = _sizeFitsContents;
                _sizeFitsContents = value;
                HandleContentFitsSizeChange(value);
                contentHasBeenSizedManually = value || oldValue;
            }
        }



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
        }

        public override void Start()
        {
            base.Start();
            DisplayName = "ScrollViewComponent";
        }

        public ScrollViewComponent Create(ScrollViewDirection direction, ScrollRect.MovementType movementType, bool sizeFitsContents)
        {
            _scroll.horizontal = direction.HasFlag(ScrollViewDirection.Horizontal);
            _scroll.vertical = direction.HasFlag(ScrollViewDirection.Vertical);

            _scroll.movementType = movementType;

            SizeFitsContents = sizeFitsContents;
            return this;
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

        public ScrollViewComponent SizeContent(float x, float y)
        {
            content.Size(x, y);
            contentHasBeenSizedManually = true;
            return this;
        }

        public void HandleContentFitsSizeChange(bool active)
        {
            if (_fitter)
            {
                _fitter.enabled = active;
                _fitter.horizontalFit= _scroll.horizontal ? ContentSizeFitter.FitMode.MinSize : ContentSizeFitter.FitMode.Unconstrained;
                _fitter.verticalFit = _scroll.vertical ? ContentSizeFitter.FitMode.MinSize : ContentSizeFitter.FitMode.Unconstrained;
            }
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