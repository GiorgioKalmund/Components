using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components.Runtime.Components
{
    public class WindowComponent : ResizableWindowComponent 
    {

        public override void Awake()
        {
            base.Awake();
            WindowBase.Color(UnityEngine.Color.gray6);
        }

        public override void Start()
        {
            base.Start();
            DisplayName = "WindowComponent";
            
            ScrollContent.Alpha(1).Sprite("backdrop_1").ImageType(Image.Type.Tiled).PixelsPerUnitMultiplier(0.33f);
            WindowBase.Color(UnityEngine.Color.gray6);
        }
    }
}