using System;
using UnityEngine;
using UnityEngine.UI;

namespace Components.Runtime.Components
{
    [Flags]
    public enum ScrollViewDirection
    {
        Horizontal = 1,
        Vertical = 2,
        Both = Horizontal | Vertical
        
    }
    
    public class ScrollViewComponent : ImageComponent 
    {
        public ImageComponent content;

        private RectMask2D _mask;
        private ContentSizeFitter _fitter;
        private ScrollRect _scroll;

        private VerticalLayoutGroup _vStack;
        private HorizontalLayoutGroup _hStack;
        private GridLayoutGroup _grid;

        private bool _contentFitsSize;
        public bool ContentFitsSize
        {
            get => _contentFitsSize;
            set
            {
                _contentFitsSize = value;
                HandleContentFitsSizeChange(value);
            }
        }


        public override void Awake()
        {
            base.Awake();

            _scroll = gameObject.AddComponent<ScrollRect>();
            _mask = gameObject.AddComponent<RectMask2D>();

            content = ComponentBuilder.N<ImageComponent>("Content", transform)
                ;
            _fitter = content.gameObject.AddComponent<ContentSizeFitter>();

            _scroll.content = content.GetRect();
        }

        public override void Start()
        {
            base.Start();
            DisplayName = "ScrollViewComponent";
        }

        public ScrollViewComponent Create(ScrollViewDirection direction, ScrollRect.MovementType movementType, bool contentFitsSize)
        {
            _scroll.horizontal = direction.HasFlag(ScrollViewDirection.Horizontal);
            _scroll.vertical = direction.HasFlag(ScrollViewDirection.Vertical);

            _scroll.movementType = movementType;

            ContentFitsSize = contentFitsSize;
            return this;
        }

        public ScrollViewComponent AddVerticalLayout(float spacing, TextAnchor childAlignment = TextAnchor.MiddleCenter, bool forceExpandChildren = false)
        {
            _vStack = AddLayout<VerticalLayoutGroup>(spacing, childAlignment, forceExpandChildren);
            return this;
        }
        
        public ScrollViewComponent AddHorizontalLayout(float spacing, TextAnchor childAlignment = TextAnchor.MiddleCenter, bool forceExpandChildren = false)
        {
            _hStack = AddLayout<HorizontalLayoutGroup>(spacing, childAlignment, forceExpandChildren);
            return this;
        }

        private T AddLayout<T>(float spacing, TextAnchor childAlignment = TextAnchor.MiddleCenter, bool forceExpandChildren = false) where T : HorizontalOrVerticalLayoutGroup
        {
            var layout = content.gameObject.AddComponent<T>();
            layout.spacing = spacing;
            layout.childAlignment = childAlignment;
            
            if (!forceExpandChildren)
            {
                
                layout.childControlWidth = false;
                layout.childControlHeight = false;
                
                layout.childForceExpandWidth = false;
                layout.childForceExpandHeight = false;
            }
            return layout;
        }

        public ScrollViewComponent ContentPadding(PaddingSide side, int amount)
        {
            if (side.HasFlag(PaddingSide.Leading))
            {
                if (_vStack) _vStack.padding.left = amount;
                if (_hStack) _hStack.padding.left = amount;
            }

            if (side.HasFlag(PaddingSide.Trailing))
            {
                if (_vStack) _vStack.padding.right = amount;
                if (_hStack) _hStack.padding.right = amount;
            }

            if (side.HasFlag(PaddingSide.Top))
            {
                if (_vStack) _vStack.padding.top = amount;
                if (_hStack) _hStack.padding.top = amount;
            }

            if (side.HasFlag(PaddingSide.Bottom))
            {
                if (_vStack) _vStack.padding.bottom = amount;
                if (_hStack) _hStack.padding.bottom = amount;
            }
            
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
            return this;
        }

        public ScrollViewComponent SizeContent(float x, float y)
        {
            content.Size(x, y);
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
    }
}