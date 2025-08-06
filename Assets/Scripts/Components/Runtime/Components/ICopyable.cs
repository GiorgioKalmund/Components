using UnityEngine;
using UnityEngine.UI;

namespace Components.Runtime.Components
{
    public interface ICopyable<T> where T : BaseComponent
    {
        T Copy(bool fullyCopyRect = true);
        T CopyFrom(T other, bool fullyCopyRect = true);
    }
    
    public static class CopyableExtensions
    {
        public static T BaseCopy<T>(this BaseComponent src, T component) where T : BaseComponent
        {
            GameObject copy = ComponentBuilder
                .CreateEmptyGameObjectWithParent(
                    component.GetParent(),
                    false,
                    component.DisplayName
                );
            var copyComponent = copy.AddComponent<T>();

            return copyComponent;
        }

    }
}