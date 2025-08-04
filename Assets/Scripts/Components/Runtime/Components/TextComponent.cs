using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using FontStyles = TMPro.FontStyles;

namespace Components.Runtime.Components
{
    public class TextComponent : BaseComponent
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

        public TextComponent FontStyle(TMPro.FontStyles style)
        {
            _textMesh.fontStyle = style;
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

    }
}