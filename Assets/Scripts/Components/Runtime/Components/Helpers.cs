using UnityEngine;

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
    }

    public static class BehaviourHelper
    {
        public static Transform GetTransform(this Behaviour behaviour)
        {
            return behaviour.gameObject.transform;
        }
    }
}