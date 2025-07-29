using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Components.Runtime.Components
{
    public class ButtonComponent : ImageComponent
    {
        public ButtonComponent() { NamePrefix = "ButtonComponent"; }
        
        // -- Button -- //
        protected Button ButtonElement;

        // -- Subcomponents -- // 
        protected ImageComponent ForegroundImage;
        protected TextComponent ButtonText;
        
        public override void Awake()
        {
            base.Awake();

            ButtonElement = gameObject.GetOrAddComponent<Button>();

            ForegroundImage = ComponentBuilder.N<ImageComponent>(transform)
                    .RaycastTarget(false)
                    .Sprite("a")
                ;

            ButtonText = ComponentBuilder.N<TextComponent>(transform)
                    .AlignCenter()
                    .VAlignCenter()
                    .Color(UnityEngine.Color.gray1)
                ;
        }

        public ButtonComponent Create(string text, UnityAction action = null, Sprite foreground = null)
        {
            Text(text);
            if (action != null)
                Function(action);
            if (foreground)
                ForegroundImage.Sprite(foreground);
            ForegroundImage.Alpha(foreground ? 1 : 0);
            
            return this;
        }

        public ButtonComponent Text(string text)
        {
            ButtonText.Text(text);
            return this;
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
    }
}