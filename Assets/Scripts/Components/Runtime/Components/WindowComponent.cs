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
            
            ScrollContent.Alpha(1).Sprite("gui_assets","tileable_backdrop").ImageType(Image.Type.Tiled).PixelsPerUnitMultiplier(0.33f);
            WindowBase.Color(Color.gray2);
            Header.Color(Color.gray1);
        }

        public override void Start()
        {
            base.Start();
            DisplayName = "WindowComponent";
        }
    }
}