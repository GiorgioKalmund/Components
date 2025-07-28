using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

namespace Components.Runtime.Components
{
    public class ImageComponent : BaseComponent 
    {
        private Image _image;
        protected static string NamePrefix = "ImageComponent";
        protected readonly static Vector2 DefaultSize = new Vector2(100, 100);

        public override void Awake()
        {
            base.Awake();
            _image = gameObject.GetOrAddComponent<Image>();
            DisplayName = NamePrefix;
            this.Size(DefaultSize);
        }

        public ImageComponent Sprite(Sprite sprite) 
        {
            _image.sprite = sprite;
            // Only set if not already set from somewhere else
            if (string.IsNullOrEmpty(DisplayName))
                DisplayName = NamePrefix + ": " + sprite.name;
            return this;
        }
        public ImageComponent Sprite(Texture2D texture2D)
        {
            return Sprite(texture2D.ToSprite());
        }
        
        public ImageComponent Sprite(string path)
        {
            return Sprite(ImageService.GetSprite(path));
        }
        
        public ImageComponent Color(Color color, bool keepPreviousAlphaValue = false) 
        {
            if (keepPreviousAlphaValue)
            {
                var prevAlpha = _image.color.a;
                _image.color = color;
                Alpha(prevAlpha);
            }
            else
            {
                _image.color = color;
            }
            return this;
        }
        
        public ImageComponent Alpha(float alpha)
        {
            var color = _image.color;
            color.a = alpha;
            _image.color = color;
            return this;
        }
        
        public ImageComponent ImageType(Image.Type imageType)
        {
            _image.type = imageType;
            return this;
        }
        
        public ImageComponent PixelsPerUnitMultiplier(float multiplier)
        {
            _image.pixelsPerUnitMultiplier = multiplier;
            return this;
        }

        public Image GetImage()
        {
            return _image;
        }

        public ImageComponent RaycastTarget(bool target)
        {
            _image.raycastTarget = target;
            return this;
        }
    }

}