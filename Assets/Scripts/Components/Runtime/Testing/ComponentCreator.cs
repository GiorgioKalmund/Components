using System;
using Components.Runtime.Components;
using Components.Runtime.Input;
using UnityEngine;
using UnityEngine.InputSystem;
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
            var button = ComponentBuilder.N<ButtonComponent>("Button", GUIService.GetCanvas().GetTransform())
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
                .SizeContent(800, 2000)
                .AddVerticalLayout(30)
                .ContentPadding(PaddingSide.Bottom, 100)
                ;

            var a = ComponentBuilder.N<InputComponent>(GUIService.GetCanvas().GetTransform())
                    .Create("Hello, World!")
                    .Color(Color.black, Color.gray5)
                    .Size(300, 50)
                ;

            window3.AddContent(a);
            /*
            _copyButton = ComponentBuilder.N<ButtonComponent>("Inside")
                    .Size(200, 100)
                    .Create("Inside!", focusable:true)
                    .Function(SpawnNew)
                    .Color(Color.red)
                    .Cast<ButtonComponent>()
                ;
            _copyButton.GetTextComponent().Bold();
            window3.AddContent(_copyButton);
            */

        }

        private void SpawnNew()
        {
            var b1 = _copyButton.Copy();
            var b2 = _copyButton.Copy();

            window3.AddContent(b1);
            window3.AddContent(b2);
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