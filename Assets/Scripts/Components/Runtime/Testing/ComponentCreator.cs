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
                    .Color(Color.magenta)
                    .Size(500, 300)
                    .Cast<ImageComponent, WindowComponent>()
                    .Offset(250, 0)
                ;
            
            
            WindowComponent window2 = ComponentBuilder.N<WindowComponent>(GUIService.GetCanvas().GetTransform())
                    .Build("Window 2")
                    .ContentPadding(5)
                    .Color(Color.green)
                    .Size(500, 300)
                    .Cast<ImageComponent, WindowComponent>()
                ;

            var a = ComponentBuilder.N<ImageComponent>("Hello!")
                    .Color(Color.red)
                ;

            window2.AddContent(a);
        }
    }
}