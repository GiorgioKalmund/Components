using UnityEngine;

namespace Components.Runtime.Service
{
    public static class Cursor
    {
        public static void ChangeCursor(Sprite sprite, Vector2 hotspot, CursorMode mode)
        {
            UnityEngine.Cursor.SetCursor(sprite.texture, hotspot, mode);
        }
    }
}