using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Components.Runtime.Components
{
    public class TextComponent : BaseComponent
    {
        private TextMeshProUGUI _textMesh;
        protected static string NamePrefix = "TextComponent";
        private readonly Vector2 _defaultSize = new Vector2(100, 100);
        
        
        public override void Awake()
        {
            base.Awake();
            _textMesh = gameObject.GetOrAddComponent<TextMeshProUGUI>();
            DisplayName = NamePrefix;
            this.Size(_defaultSize);
        }

        public TextComponent Text(string text)
        {
            _textMesh.text = text;
            return this;
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
            return VAlignment(VerticalAlignmentOptions.Middle);
        }
        
        public TextComponent OverflowMode(TextOverflowModes overflowModes)
        {
            _textMesh.overflowMode = overflowModes;
            return this;
        }

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