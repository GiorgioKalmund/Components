using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Components.Runtime.Components
{
    public static class SpriteHelper
    {
        public static Sprite ToSprite(this Texture2D texture, Vector2 pivot)
        {
            Sprite toReturn = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), pivot);
            toReturn.name = texture.name;
            return toReturn;
        }
        public static Sprite ToSprite(this Texture2D texture)
        {
            return texture.ToSprite(new Vector2(0.5f, 0.5f));
        }

        public static Vector2 NativeSize(this Sprite sprite)
        {
            var nativeTex = sprite.bounds.size;
            var nativeSize = new Vector2(nativeTex.x * 100, nativeTex.y * 100);
            return nativeSize;
        }
    }

    public static class BehaviourHelper
    {
        public static Transform GetTransform(this Behaviour behaviour)
        {
            return behaviour.gameObject.transform;
        }

        public static void CopyFrom(this HorizontalLayoutGroup layout, HorizontalLayoutGroup other) 
        {
            if (!other)
                return;
            
            layout.spacing = other.spacing;
            layout.childForceExpandWidth = other.childForceExpandWidth;
            layout.childForceExpandHeight = other.childForceExpandHeight;
            layout.childControlWidth = other.childControlWidth;
            layout.childControlHeight= other.childControlHeight;

            layout.padding = other.padding.Clone();
        }
    }
}