using System;
using Components.Runtime.Components;
using Components.Runtime.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Components.Runtime.Testing
{
    public class ComponentCreator : MonoBehaviour
    {
        private ComponentControls _input;
        public void Awake()
        {
            TextComponent.GlobalFont(Resources.Load<TMPro.TMP_FontAsset>("Font/Main"));
            
            _input = new ComponentControls();
            
            var window1= ComponentBuilder.N<BaseWindowComponent>("W1", GUIService.GetCanvas().GetTransform())
                    .Build(_input.UI.ShowWindow)
                    .SetContent(Color.orange)
                    .ContentPadding(10)
                    .SetBase(Color.red)
                    .SetHeaderColor(Color.blue)
                    .Size(500, 300)
                    .Offset(0, 0)
                ;
            
            var window11 = ComponentBuilder.N<BaseWindowComponent>("W2", GUIService.GetCanvas().GetTransform())
                    .Build(_input.UI.ShowWindow)
                    .SetContent(Color.red)
                    .ContentPadding(20)
                    .Size(500, 300)
                    .NoHeader()
                    .Offset(500, 0)
                ;
            
            var window2 = ComponentBuilder.N<ResizableWindowComponent>(GUIService.GetCanvas().GetTransform())
                    .Build("Window 1 Sint voluptate enim dolor incididunt consectetur occaecat incididunt consectetur aute id exercitation. Do non aliquip ea do deserunt cupidatat velit sit pariatur sit pariatur veniam magna anim.")
                    .MinimumSize(300, 300)
                    .SetBase(Color.gray)
                    .Size(500, 300)
                    .ContentPadding(7)
                    .Cast<ResizableWindowComponent>()
                    .Offset(0, -300)
                ;
            
            var window3 = ComponentBuilder.N<WindowComponent>(GUIService.GetCanvas().GetTransform())
                    .Build("Window 2", Color.green, Color.blue)
                    .ContentPadding(5)
                    .Size(500, 300)
                    .Cast<WindowComponent>()
                    .Offset(500, -300)
                ;

            var a = ComponentBuilder.N<ButtonComponent>("Hello!")
                    .Size(100, 100)
                    .Text("Yo", Color.black)
                ;
            a.TextComponent().Bold();
            
            var b = ComponentBuilder.N<ButtonComponent>("B,A,Sports!")
                    .Size(300, 100)
                    .Text("End Game", Color.black)
                ;
            b.TextComponent().Underline();

            var popup = ComponentBuilder.N<PopupComponent>(GUIService.GetCanvas().GetTransform(), "Popup")
                    .Build(GUIService.GetCanvas())
                    .DontCloseOnBackGroundTap()
                    .AddContent(b)
                ;
            
            
            a.Function(() => popup.OpenPopup());
            b.Function(() => popup.ClosePopup());

            window1.AddContent(a);
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