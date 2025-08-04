using System;
using System.Collections;
using System.Threading.Tasks;
using Components.Runtime.Components;
using Components.Runtime.Input;
using UnityEngine;
using UnityEngine.UI;

namespace Components.Runtime.Testing
{
    public class ComponentCreator : MonoBehaviour
    {
        private ComponentControls _input;
        private WindowComponent window3;
        private ButtonComponent _copyButton;
        public void Awake()
        {
            TextComponent.GlobalFont(Resources.Load<TMPro.TMP_FontAsset>("Font/Main"));

            var backdrop = ComponentBuilder.N<ImageComponent>("Brr", GUIService.GetCanvas().GetTransform())
                    .FullScreen(GUIService.GetCanvas())
                    .Sprite("wallpaper_1")
                ;
            var focusButton = ComponentBuilder.N<ButtonComponent>("Button", GUIService.GetCanvas().GetTransform())
                .Size(300, 100)
                .Pivot(PivotPosition.MiddleRight, true)
                .Offset(-100, 0)
                .Create("Focus me", focusable:true)
                ;
            
            _input = new ComponentControls();
            
            window3 = ComponentBuilder.N<WindowComponent>(GUIService.GetCanvas().GetTransform())
                    .Build(_input.UI.ShowWindow,"Window 2", Color.green, Color.blue)
                    .StartHidden()
                    .ContentPadding(5)
                    .Size(500, 300)
                    .Cast<WindowComponent>()
                    .Offset(500, -300)
                ;
            window3.ConfigureContent()
                .Create(ScrollViewDirection.Vertical, ScrollRect.MovementType.Clamped, false)
                .SizeContent(500, 500)
                .AddVerticalLayout(30, TextAnchor.UpperLeft)
                .ContentPadding(PaddingSide.Leading, 30)
                .ContentPadding(PaddingSide.Top, 30)
                .ScrollToTop()
                ;

            var button = ComponentBuilder.N<ButtonComponent>(GUIService.GetCanvas().GetTransform())
                    .Create("Helllo, World!", focusable:true)
                    .Function(() => Debug.Log("Hello From button"))
                    .Size(300, 100)
                    .Pivot(PivotPosition.MiddleLeft, true)
                    .Cast<ButtonComponent>()
                ;

            button.GetTextComponent().Bold().Italic();

            var b = button.Copy().Function(() => Debug.Log("Hello from purple button"));
            b.Offset(200, 300).Color(Color.purple);
        }

        private void OnEnable()
        {
            _input?.Enable();
        }

        private void OnDisable()
        {
            _input?.Disable();
        }
    }
}