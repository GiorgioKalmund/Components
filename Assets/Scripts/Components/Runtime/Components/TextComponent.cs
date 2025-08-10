using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using FontStyles = TMPro.FontStyles;

namespace Components.Runtime.Components
{
    public class TextComponent : BaseComponent, ICopyable<TextComponent>
    {
        private TextMeshProUGUI _textMesh;
        protected static readonly string NamePrefix = "TextComponent";
        private readonly Vector2 _defaultSize = new Vector2(100, 100);

        private static TMP_FontAsset _globalFont;

        public static void GlobalFont(TMP_FontAsset asset)
        {
            _globalFont = asset;
        }
        
        public override void Awake()
        {
            base.Awake();
            _textMesh = gameObject.GetOrAddComponent<TextMeshProUGUI>();
            DisplayName = NamePrefix;
            this.Size(_defaultSize);

            Font(_globalFont);
        }

        public virtual void Start()
        {
            DisplayName = NamePrefix;
        }

        public TextComponent Text(string text, Color? color = null)
        {
            _textMesh.text = text;
            if (color.HasValue)
                Color(color.Value);
            return this;
        }
        
        public TextComponent Text(int text)
        {
            return Text(text.ToString());
        }

        public TextComponent Clear()
        {
            return Text("");
        }

        public string GetText()
        {
            return _textMesh.text;
        }
      
        public TextComponent Font(TMP_FontAsset font)
        {
            _textMesh.font = font;
            return this;
        }
        
        public TextComponent FontSize(float fontSize)
        {
            _textMesh.fontSize = fontSize;
            return this;
        }

        public TextComponent Alignment(TextAlignmentOptions alignmentOptions)
        {
            _textMesh.alignment = alignmentOptions;
            return this;
        }
        
        public TextComponent AlignCenter()
        {
            return Alignment(TextAlignmentOptions.Center);
        }
        
        public TextComponent VAlignment(VerticalAlignmentOptions alignmentOptions)
        {
            _textMesh.verticalAlignment = alignmentOptions;
            return this;
        }
        
        public TextComponent VAlignCenter()
        {
            return VAlignment(VerticalAlignmentOptions.Geometry);
        }
        
        public TextComponent OverflowMode(TextOverflowModes overflowModes)
        {
            _textMesh.overflowMode = overflowModes;
            return this;
        }
        
        public TextComponent WrappingMode(TextWrappingModes wrappingModes)
        {
            _textMesh.textWrappingMode = wrappingModes;
            return this;
        }
        
        public TextComponent NoWrap()
        {
            return WrappingMode(TextWrappingModes.NoWrap);
        }

        public TextComponent FontStyle(FontStyles style)
        {
            _textMesh.fontStyle |= style;
            return this;
        }
        public TextComponent FontStyleRemove(FontStyles style)
        {
            _textMesh.fontStyle &= ~style;
            return this;
        }

        public TextComponent FitToContents(bool fit = true)
        {
            _textMesh.autoSizeTextContainer = fit;
            return this;
        }

        public TextComponent Bold() { return FontStyle(FontStyles.Bold); }
        public TextComponent Italic() { return FontStyle(FontStyles.Italic); }
        public TextComponent Underline() { return FontStyle(FontStyles.Underline); }

        // Duplicate to ImageComponent implementation, maybe find some consolidated space?
        public TextComponent Color(Color color, bool keepPreviousAlphaValue = false) 
        {
            if (keepPreviousAlphaValue)
            {
                var prevAlpha = _textMesh.color.a;
                _textMesh.color = color;
                Alpha(prevAlpha);
            }
            else
            {
                _textMesh.color = color;
            }
            return this;
        }
        
        // Duplicate to ImageComponent implementation, maybe find some consolidated space?
        public TextComponent Alpha(float alpha)
        {
            var color = _textMesh.color;
            color.a = alpha;
            _textMesh.color = color;
            return this;
        }
        
        public TextMeshProUGUI GetTextMesh()
        {
            return _textMesh;
        }

        public TextComponent Copy(bool fullyCopyRect = true)
        {
            TextComponent textCopy = this.BaseCopy(this);
            return textCopy.CopyFrom(this, fullyCopyRect);
        }

        public TextComponent CopyFrom(TextComponent other, bool fullyCopyRect = true)
        {
            CopyLayoutElement(other, this);
            CopyRect(other.GetRect(), this, fullyCopyRect);
            CopyTextProperties(other.GetTextMesh(), this);
            return this;
        }

        public static void CopyTextProperties(TMP_Text text, TextComponent textComponent)
        {
            textComponent.Text(text.text);
            textComponent.Color(text.color);
            textComponent.Alignment(text.alignment);
            textComponent.VAlignment(text.verticalAlignment);
            textComponent.FontStyle(text.fontStyle);
            textComponent.FontSize(text.fontSize);
            textComponent.OverflowMode(text.overflowMode);
            textComponent.FitToContents(text.autoSizeTextContainer);
            if (text.enableAutoSizing)
                textComponent.AutoSize(text.fontSizeMin, text.fontSizeMax);
        }

        public TextComponent AutoSize(float minSize = 18, float maxSize = 72)
        {
            _textMesh.enableAutoSizing = true;
            _textMesh.fontSizeMin = minSize;
            _textMesh.fontSizeMax = maxSize;
            return this;
        }
    }
}