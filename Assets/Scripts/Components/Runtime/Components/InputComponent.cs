using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Components.Runtime.Components
{
    public class InputComponent : BaseComponent
    {
        // -- TMP Input Field -- //
        protected TMP_InputField Input;
        // -- Subcomponents -- //
        protected ImageComponent Backdrop;
        protected TextComponent TextContents;
        protected TextComponent HintContents;

        public override void Awake()
        {
            base.Awake();
            
            Input = gameObject.GetOrAddComponent<TMP_InputField>();
            
            Backdrop= ComponentBuilder.N<ImageComponent>(transform);

            TextContents = ComponentBuilder.N<TextComponent>(Backdrop);

            HintContents = ComponentBuilder.N<TextComponent>(Backdrop);

            Input.targetGraphic = Backdrop.GetImage();
            Input.textComponent = TextContents.GetTextMesh();
            Input.textViewport = GetRect();
            Input.placeholder = HintContents.GetTextMesh();

            TextContents.OverflowMode(TextOverflowModes.Ellipsis);
            HintContents.OverflowMode(TextOverflowModes.Ellipsis);
        }


        public InputComponent Create(string placeholder, TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard)
        {
            ContentType(contentType);
            HintContents.Text(placeholder);
            return this;
        }

        public InputComponent Color(Color textColor, Color? hintColor = null)
        {
            TextContents.Color(textColor);
            HintContents.Color(hintColor ?? textColor);
            return this;
        }

        public InputComponent ColorBackdrop(Color color)
        {
            Backdrop.Color(color);
            return this;
        }
        
        public InputComponent FontSize(float size)
        {
            TextContents.FontSize(size);
            HintContents.FontSize(size);
            return this;
        }

        public InputComponent Clear()
        {
            Input.text = "";
            return this;
        }

        public InputComponent ContentType(TMP_InputField.ContentType contentType)
        {
            Input.contentType = contentType;
            return this;
        }

        public override BaseComponent HandleSizeChanged(float x, float y)
        {
            base.HandleSizeChanged(x, y);

            Backdrop.Size(x, y);
            TextContents.Size(x, y);
            HintContents.Size(x, y);
            return this;
        }

        public TextComponent GetTextComponent() { return TextContents; }
        public string GetText() { return TextContents.GetText(); }
        public TextComponent GetHintComponent() { return HintContents; }

        private void Start()
        {
            DisplayName = "Input";
        }

        public TMP_InputField GetInput()
        {
            return Input;
        }
    }
}