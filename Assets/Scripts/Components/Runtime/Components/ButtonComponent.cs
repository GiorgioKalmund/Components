using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Components.Runtime.Components
{
    public class ButtonComponent : ImageComponent, IFocusable, ICopyable<ButtonComponent>
    {
        public ButtonComponent() { NamePrefix = "ButtonComponent"; }
        
        // -- Button -- //
        protected Button ButtonElement;
        protected List<UnityAction> Listeners = new List<UnityAction>();

        // -- Subcomponents -- // 
        protected ImageComponent ForegroundImage;
        protected TextComponent ButtonText;
        
        // -- Auto Sizing -- //
        private HorizontalLayoutGroup HorizontalLayout;
        private ContentSizeFitter _fitter;
        private LayoutElement _foregroundLayout;

        private bool _focusable;
        
        public override void Awake()
        {
            base.Awake();

            ButtonElement = gameObject.GetOrAddComponent<Button>();

            ForegroundImage = ComponentBuilder.N<ImageComponent>(transform, "Foreground-Hint")
                    .RaycastTarget(false)
                    .SetActive(false)
                ;

            ButtonText = ComponentBuilder.N<TextComponent>(transform, "Text")
                    .AlignCenter()
                    .VAlignCenter()
                    .Color(UnityEngine.Color.gray1)
                ;

            _foregroundLayout = ForegroundImage.GetOrAddComponent<LayoutElement>();
        }

        public override void Start()
        {
            base.Start();
            DisplayName = "ButtonComponent";

            if (_focusable)
                Function(this.Focus);
        }

        public ButtonComponent Create(string text = "", UnityAction action = null, Sprite foreground = null, bool focusable = false)
        {
            Text(text);
            if (action != null)
                Function(action);
            if (foreground)
            {
                Foreground(foreground);
            }

            _focusable = focusable;
            
            return this;
        }
        
        public ButtonComponent Foreground(Sprite sprite, float alpha = 1f)
        {
            ForegroundImage.Sprite(sprite).Alpha(alpha).SetActive();
            return this;
        }

        public ButtonComponent FitToContents(PaddingSide side, int amount, float spacing = 0f)
        {
            ForegroundSize(ForegroundImage.GetSize());
            
            HorizontalLayout = gameObject.GetOrAddComponent<HorizontalLayoutGroup>();
            HorizontalLayout.childForceExpandWidth = false;
            HorizontalLayout.childForceExpandHeight = false;
            HorizontalLayout.childAlignment = TextAnchor.MiddleCenter;
            
            _fitter = gameObject.GetOrAddComponent<ContentSizeFitter>();
            _fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            _fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            ContentPadding(side, amount);
            ContentSpacing(spacing);
            return this;
        }

        public ButtonComponent FitToContents(int padding = 0, float spacing = 0f)  
        {
            return FitToContents(PaddingSide.All, padding, spacing);
        }
        
        public ButtonComponent ForegroundSize(float x, float y)
        {
            _foregroundLayout.preferredWidth = x;
            _foregroundLayout.preferredHeight = y;
            return this;
        }

        public ButtonComponent ForegroundSize(Vector2 size)
        {
            return ForegroundSize(size.x, size.y);
        }

        public ButtonComponent ContentSpacing(float spacing)
        {
            HorizontalLayout.spacing = spacing;
            return this;
        }
        
        public ButtonComponent ContentPadding(PaddingSide side, int amount)
        {
            if (side.HasFlag(PaddingSide.Leading)) { if (HorizontalLayout) HorizontalLayout.padding.left = amount; }
            if (side.HasFlag(PaddingSide.Trailing)) { if (HorizontalLayout) HorizontalLayout.padding.right = amount; }
            if (side.HasFlag(PaddingSide.Top)) { if (HorizontalLayout) HorizontalLayout.padding.top = amount; }
            if (side.HasFlag(PaddingSide.Bottom)) { if (HorizontalLayout) HorizontalLayout.padding.bottom = amount; }
            return this;
        }

        public ButtonComponent Text(string text, Color? color = null)
        {
            ButtonText.Text(text, color);
            return this;
        }

        public TextComponent GetTextComponent()
        {
            return ButtonText;
        }

        public ButtonComponent Function(UnityAction action)
        {
            ButtonElement.onClick.AddListener(action);
            Listeners.Add(action);
            return this;
        }
        
        public ButtonComponent RemoveFunction(UnityAction action)
        {
            ButtonElement.onClick.RemoveListener(action);
            return this;
        }
        
        public ButtonComponent ClearAllFunctions()
        {
            ButtonElement.onClick.RemoveAllListeners();
            return this;
        }

        public override BaseComponent HandleSizeChanged(float x, float y)
        {
            base.HandleSizeChanged(x, y);
            if (ForegroundImage)
                ForegroundImage.Size(x, y);
            if (ButtonText)
                ButtonText.Size(x, y);
            return this;
        }

        public ButtonComponent HighlightedColor(Color color)
        {
            var colors = ButtonElement.colors;
            colors.highlightedColor = color;
            ButtonElement.colors = colors;
            return this;
        }
        
        public ButtonComponent PressedColor(Color color)
        {
            var colors = ButtonElement.colors;
            colors.pressedColor = color;
            ButtonElement.colors = colors;
            return this;
        }
        
        public ButtonComponent DisabledColor(Color color)
        {
            var colors = ButtonElement.colors;
            colors.disabledColor = color;
            ButtonElement.colors = colors;
            return this;
        }

        public ButtonComponent Lock()
        {
            ButtonElement.interactable = false;
            return this;
        }

        public ButtonComponent Unlock()
        {
            ButtonElement.interactable = true;
            return this;
        }

        public new ButtonComponent Copy(bool fullyCopyRect = true)
        {
            ButtonComponent copyButton = this.BaseCopy(this);
            return copyButton.CopyFrom(this, fullyCopyRect);
        }

        public bool IsFocusable()
        {
            return _focusable;
        }

        public ButtonComponent CopyFrom(ButtonComponent other, bool fullyCopyRect = true)
        {
            base.CopyFrom(other, fullyCopyRect);
            Create(focusable:other.IsFocusable());
            ButtonText.CopyFrom(other.ButtonText);
            ForegroundImage.CopyFrom(other.ForegroundImage);

            if (other._fitter || other.HorizontalLayout)
            {
                FitToContents();
                RectOffset offset = other.HorizontalLayout.padding.Clone();
                HorizontalLayout.padding = offset;
            }
            
            ClearAllFunctions();
            foreach (var unityAction in other.Listeners)
            {
                Function(unityAction);
            }

            return this;
        }
    }
}