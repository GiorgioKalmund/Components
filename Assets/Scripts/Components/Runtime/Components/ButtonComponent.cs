using System;
using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Components.Runtime.Components
{
    public class ButtonComponent : ImageComponent, IFocusable
    {
        public ButtonComponent() { NamePrefix = "ButtonComponent"; }
        
        // -- Button -- //
        protected Button ButtonElement;

        // -- Subcomponents -- // 
        protected ImageComponent ForegroundImage;
        protected TextComponent ButtonText;
        
        // -- Auto Sizing -- //
        private HorizontalLayoutGroup _hStack;
        private ContentSizeFitter _fitter;

        private bool _focusable;
        
        public override void Awake()
        {
            base.Awake();

            ButtonElement = gameObject.GetOrAddComponent<Button>();

            ForegroundImage = ComponentBuilder.N<ImageComponent>(transform, "Foreground-Hint")
                    .RaycastTarget(false)
                    .Alpha(0)
                ;

            ButtonText = ComponentBuilder.N<TextComponent>(transform, "Text")
                    .AlignCenter()
                    .VAlignCenter()
                    .Color(UnityEngine.Color.gray1)
                ;
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
                ForegroundImage.Sprite(foreground).Alpha(1);
            }

            _focusable = focusable;
            
            return this;
        }

        public ButtonComponent FitToContents(PaddingSide side, int amount)
        {
            _hStack = gameObject.AddComponent<HorizontalLayoutGroup>();
            
            _fitter = gameObject.AddComponent<ContentSizeFitter>();
            _fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            _fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            ContentPadding(side, amount);
            return this;
        }

        public ButtonComponent FitToContents(int amount = 0)
        {
            return FitToContents(PaddingSide.All, amount);
        }
        
        public ButtonComponent ContentPadding(PaddingSide side, int amount)
        {
            if (side.HasFlag(PaddingSide.Leading)) { if (_hStack) _hStack.padding.left = amount; }
            if (side.HasFlag(PaddingSide.Trailing)) { if (_hStack) _hStack.padding.right = amount; }
            if (side.HasFlag(PaddingSide.Top)) { if (_hStack) _hStack.padding.top = amount; }
            if (side.HasFlag(PaddingSide.Bottom)) { if (_hStack) _hStack.padding.bottom = amount; }
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
            //Debug.Log("Size Changed to "+ x + "x" + y);
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
    }
}