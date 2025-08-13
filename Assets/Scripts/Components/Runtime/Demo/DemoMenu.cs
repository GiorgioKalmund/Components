using System;
using Components.Runtime.Components;
using Components.Runtime.Components.Screen;
using UnityEngine;
using UnityEngine.UI;

namespace Components.Runtime.Demo
{
    public class DemoMenu : SimpleScreen
    {
        public override void Setup()
        {
            DisplayName = "DemoMenu";
            
            var backdrop = ComponentBuilder.N<ImageComponent>("Backdrop", transform)
                    .FullScreen(canvas)
                    .Sprite("wallpaper_dark")
                ;

            var title = ComponentBuilder.N<TextComponent>("Title", transform)
                    .Text("Component: UGUI Demo").FitToContents().FontSize(70).Bold()
                    .Pivot(PivotPosition.UpperCenter, true).Offset(0, -50)
                ;

            var buttonParent = ComponentBuilder.N<ImageComponent>("Menu Buttons", transform)
                    .Size(1200, 800)
                    .Offset(0, -65)
                    .Color(Color.gray1, 0.5f)
                    .AddVerticalLayout(childAlignment:TextAnchor.UpperLeft, spacing:25)
                    .ContentPadding(25, ScrollViewDirection.Vertical)
                ;
            
            var sceneButton1 = ComponentBuilder.N<ButtonComponent>("Scene 1").Parent(buttonParent)
                    .Size(500, 100)
                    .Create("Scene 1", foreground:ImageService.GetSpriteFromAsset("player", "head"))
                    .SpriteSwap(ImageService.GetSpriteDirectly("Slice/Slot Selected"))
                    .Sprite(ImageService.GetSpriteDirectly("Slice/Slot")).ImageType(Image.Type.Sliced).PixelsPerUnitMultiplier(0.33f)
                    .AddHorizontalLayout(childAlignment:TextAnchor.MiddleLeft, spacing:10).ContentPadding(PaddingSide.Leading, 25, ScrollViewDirection.Horizontal)
                    .Pivot(PivotPosition.MiddleLeft, true)
                    .Cast<ButtonComponent>()
                ;
            sceneButton1.GetTextComponent().FitToContents().Bold().Color(Color.white);
            sceneButton1.GetForeground().Size(60, 60);

            var sceneButton2 = sceneButton1.Copy().Text("Scene 2").Foreground(ImageService.GetSpriteFromAsset("player", "booster paddels single"));
        }

        public override Canvas GetCanvas()
        {
            return GUIService.GetCanvas();
        }
    }
}