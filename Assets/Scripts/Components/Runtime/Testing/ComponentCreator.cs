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
            _input = new ComponentControls();
            
            ResizableWindowComponent window1 = ComponentBuilder.N<WindowComponent>("W1", GUIService.GetCanvas().GetTransform())
                    .Build(_input.UI.ShowWindow, "Window 1")
                    .Size(500, 300)
                    .Cast<WindowComponent>()
                    .Offset(250, -250)
                ;
            
            ResizableWindowComponent window2 = ComponentBuilder.N<WindowComponent>(GUIService.GetCanvas().GetTransform())
                    .Build("Window 1 Sint voluptate enim dolor incididunt consectetur occaecat incididunt consectetur aute id exercitation. Do non aliquip ea do deserunt cupidatat velit sit pariatur sit pariatur veniam magna anim.")
                    .Size(500, 300)
                    .ContentPadding(5)
                    .Cast<WindowComponent>()
                    .Offset(500, -500)
                ;
            
            ResizableWindowComponent window3 = ComponentBuilder.N<ResizableWindowComponent>(GUIService.GetCanvas().GetTransform())
                    .Build("Window 2", Color.green, Color.blue)
                    .ContentPadding(5)
                    .Size(500, 300)
                    .Cast<ResizableWindowComponent>()
                    .Offset(750, -750)
                ;

            var a = ComponentBuilder.N<ButtonComponent>("Hello!")
                    .Size(300, 100)
                    .Color(Color.gray)
                    .Sprite("missing")
                ;

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