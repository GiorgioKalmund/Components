using Components.Runtime.Components;
using UnityEngine;

namespace Components.Runtime.Testing
{
    public class ComponentCreator : MonoBehaviour
    {
        public void Awake()
        {
            WindowComponent window1 = ComponentBuilder.N<WindowComponent>(GUIService.GetCanvas().GetTransform())
                    .Build("Window 1")
                    .Size(500, 300)
                    .Cast<WindowComponent>()
                    .Offset(250, -250)
                ;
            
            WindowComponent window2 = ComponentBuilder.N<WindowComponent>(GUIService.GetCanvas().GetTransform())
                    .Build("Window 1 Sint voluptate enim dolor incididunt consectetur occaecat incididunt consectetur aute id exercitation. Do non aliquip ea do deserunt cupidatat velit sit pariatur sit pariatur veniam magna anim.")
                    .Size(500, 300)
                    .Cast<WindowComponent>()
                    .Offset(500, -500)
                ;
            
            WindowComponent window3 = ComponentBuilder.N<WindowComponent>(GUIService.GetCanvas().GetTransform())
                    .Build("Window 2")
                    .ContentPadding(5)
                    .Size(500, 300)
                    .Cast<WindowComponent>()
                    .Offset(750, -750)
                ;

            var a = ComponentBuilder.N<ButtonComponent>("Hello!")
                    .Size(300, 100)
                    .Color(Color.gray)
                ;

            window1.AddContent(a);
        }
    }
}