using System.Collections.Generic;
using System.Linq;
using Components.Runtime.Components;
using UnityEngine;

public class ImageService
{
    public static readonly string MissingTextureLocation = "Textures/missing";
    public static Texture2D GetTexture2D(string path, FilterMode filterMode = FilterMode.Point)
    {
        if (string.IsNullOrEmpty(path))
        {
            return Resources.Load<Texture2D>(MissingTextureLocation);
        }
        Texture2D texture = Resources.Load<Texture2D>(path);
        if (!texture)
        {
            Debug.LogWarning("No image found for: " + path);
            texture = Resources.Load<Texture2D>(MissingTextureLocation);
        }
        texture.filterMode = filterMode;
        return texture;
    }
    
    public static Sprite GetSpriteDirectly(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return GetSprite(MissingTextureLocation);
        }
        Sprite sprite = Resources.Load<Sprite>(path);
        if (!sprite)
        {
            return GetSprite(MissingTextureLocation);
        }
        return sprite;
    }
    
    public static Sprite GetSprite(string path, string resourceFolder = "Textures")
    {
        Texture2D texture = GetTexture2D(resourceFolder + "/" + path);
        return texture.ToSprite();
    }
  
}
