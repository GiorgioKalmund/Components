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
            layout.reverseArrangement = other.reverseArrangement;

            layout.padding = other.padding.Clone();
        }

        public static void ReverseArrangement(this HorizontalLayoutGroup layout, bool reverse = true)
        {
            layout.reverseArrangement = reverse;
        }
    }

    // Scuffed but works for now. (0,0) is at the bottom left corner of the canvas. 
    public static class GameObjectScreenSpaceHelper
    {
        public static Vector2 ToCanvasPos(this Vector3 worldPos, Camera camera, Canvas canvas)
        {
            var sp = camera.WorldToScreenPoint(worldPos);
            return new Vector2(sp.x / canvas.GetWidth() * canvas.pixelRect.width,
                sp.y / canvas.GetHeight() * canvas.pixelRect.height);
        }

        public static Vector3 ToWorldPos(this Vector2 canvasPos, Camera camera, Canvas canvas, float cameraDepth)
        {
            var screenPos = new Vector3(
                canvasPos.x / canvas.pixelRect.width  * canvas.GetWidth(),
                canvasPos.y / canvas.pixelRect.height * canvas.GetHeight(),
                cameraDepth
            );
            return camera.ScreenToWorldPoint(screenPos);
        }

        public static void MoveToCanvasPos(this GameObject obj, Vector2 canvasPos, Camera camera, Canvas canvas)
        {
            float depth = camera.WorldToScreenPoint(obj.transform.position).z;
            obj.transform.position = canvasPos.ToWorldPos(camera, canvas, depth);
        }
    }

    public static class CanvasHelper
    {
        public static Vector2 GetSize(this Canvas c)
        {
            return c.GetComponent<RectTransform>().sizeDelta;
        }

        public static float GetWidth(this Canvas c)
        {
            return c.GetSize().x;
        }
        public static float GetHeight(this Canvas c)
        {
            return c.GetSize().y;
        }
    }
}