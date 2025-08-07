using Components.Runtime.Input;
using UnityEngine;

namespace Components.Runtime.Service
{
    public class InputService 
    {
        public static ComponentControls Input;

        public InputService()
        {
            if (Input == null)
            {
                Input = new ComponentControls();
            }
            Input?.Enable();
        }
        
        ~InputService()
        {
            Input?.Disable();
        }
    }
}