using Components.Runtime.Components.Animation;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Components.Runtime.Components
{
    public class ImageComponent : BaseComponent, ICopyable<ImageComponent>, IPointerEnterHandler, IPointerExitHandler
    {
        private Image _image;
        protected static string NamePrefix = "ImageComponent";
        protected readonly static Vector2 DefaultSize = new Vector2(100, 100);
        
        private InputAction _toggleInputAction;
        
        private LayoutElement _foregroundLayout;
        protected SpriteAnimator Animator;

        public override void Awake()
        {
            base.Awake();
            _image = gameObject.GetOrAddComponent<Image>();
            this.Size(DefaultSize);
        }

        public virtual void Start()
        {
            this.SafeDisplayName(NamePrefix);

            if (_toggleInputAction != null)
                _toggleInputAction.performed += ToggleVisibility;
        }

        public ImageComponent Sprite(Sprite sprite, Image.Type? imageType = null) 
        {
            if (imageType.HasValue)
                ImageType(imageType.Value);
            _image.sprite = sprite;
            // Only set if not already set from somewhere else
            this.SafeDisplayName(NamePrefix + ": " + sprite?.name);
            return this;
        }
        public ImageComponent Sprite(Texture2D texture2D)
        {
            return Sprite(texture2D.ToSprite());
        }
        
        public ImageComponent Sprite(string path, Image.Type? imageType = null)
        {
            return Sprite(ImageService.GetSprite(path), imageType);
        }
        
        public ImageComponent Sprite(string asset, string layerName, Image.Type? imageType = null)
        {
            return Sprite(ImageService.GetSpriteFromAsset(asset, layerName), imageType);
        }

        public ImageComponent ClearSprite()
        {
            return Sprite((Sprite)null);
        }

        public ImageComponent NativeSize(Vector2 scale)
        {
            return this.Size(_image.sprite.NativeSize() * scale);
        }
        public ImageComponent NativeSize()
        {
            return NativeSize(Vector2.one);
        }
        public ImageComponent NativeSize(float scaleFactorX, float scaleFactorY)
        {
            return NativeSize(new Vector2(scaleFactorX, scaleFactorY));
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

        public ImageComponent Color(Color color, float alpha)
        {
            return Color(color).Alpha(alpha);
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

        public ImageComponent ImageTypeFilled(Image.FillMethod method, float amount = 1f)
        {
            ImageType(Image.Type.Filled);
            _image.fillMethod = method;
            FillAmount(amount);
            return this;
        }

        public ImageComponent FillAmount(float amount)
        {
            _image.fillAmount = amount;
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

        public bool HasImage()
        {
            return _image.sprite;
        }

        public new ImageComponent Copy(bool fullyCopyRect = true)
        {
            ImageComponent copyImage = this.BaseCopy(this);
            return copyImage.CopyFrom(this, fullyCopyRect);
        }

        public ImageComponent CopyFrom(ImageComponent other, bool fullyCopyRect = true)
        {
            base.CopyFrom(other, fullyCopyRect);
            //DisplayName = other.DisplayName + " (Copy)";
            CopyImageProperties(other.GetImage(), this);
            CopyAnimator(other, this);
            CopyMaterial(other, this);
            return this;
        }

        public static void CopyMaterial(ImageComponent other, ImageComponent copyImage)
        {
            var defaultMat = Graphic.defaultGraphicMaterial;
            if (other.GetImage().material && other.GetImage().material != defaultMat)
            {
                copyImage.Material(other.GetImage().material);
            }
        }

        public static void CopyAnimator(ImageComponent other, ImageComponent copyImage)
        {
            if (other.Animator)
            {
                copyImage.AddAnimator();
                copyImage.Animator.CopyFrom(other.Animator);
            }
        }


        public ImageComponent MakeLayoutElement(float preferredWidth, float preferredHeight)
        {
            _foregroundLayout = gameObject.GetOrAddComponent<LayoutElement>();
            _foregroundLayout.preferredWidth = preferredWidth;
            _foregroundLayout.preferredHeight = preferredHeight;
            return this;
        }

        public static void CopyImageProperties(Image image, ImageComponent copyImage) 
        {
            copyImage.ImageType(image.type);
            copyImage.Color(image.color);
            copyImage.Sprite(image.sprite);
            copyImage.PixelsPerUnitMultiplier(image.pixelsPerUnitMultiplier);
            copyImage.RaycastTarget(image.raycastTarget);
        }

        public ImageComponent ToggleVisibilityUsing(InputAction action)
        {
            _toggleInputAction = action;
            _toggleInputAction?.Enable();
            return this;
        }

        public void ToggleVisibility(InputAction.CallbackContext callbackContext)
        {
            GetImage().enabled = !GetImage().enabled;
        }

        private void OnEnable()
        {
            _toggleInputAction?.Enable();
        }

        private void OnDisable()
        {
            _toggleInputAction?.Disable();
        }

        // For more flexible and efficient use, allowing controlling of instances, we might want to use MaterialPropertyBlock in the future
        public ImageComponent Material(Material material)
        {
            _image.material = material;
            return this;
        }

        public SpriteAnimator AddAnimator()
        {
            Animator = gameObject.AddComponent<SpriteAnimator>();
            Animator.DisplayName(DisplayName);
            return Animator;
        }

        public SpriteAnimator GetAnimator()
        {
            return Animator;
        }

        public HorizontalLayoutGroup GetHorizontalLayout()
        {
            return HorizontalLayout;
        }
        
        
        public virtual void HandlePointerEnter(PointerEventData eventData) { }
        public virtual void HandlePointerExit(PointerEventData eventData) { }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            HandlePointerEnter(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HandlePointerExit(eventData);
        }
    }

}