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
        
        public override void Awake()
        {
            base.Awake();

            ButtonElement = gameObject.GetOrAddComponent<Button>();

            ForegroundImage = ComponentBuilder.N<ImageComponent>(transform)
                    .RaycastTarget(false)
                    .Sprite("a")
                ;
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

        public ButtonComponent Size(float x, float y)
        {
            this.Size(new Vector2(x, y));
            ForegroundImage.Size(x, y);
            return this;
        }
    }
}