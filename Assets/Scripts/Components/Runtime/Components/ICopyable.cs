using UnityEngine;

namespace Components.Runtime.Components
{
    public interface ICopyable<T> where T : BaseComponent
    {
        T Copy();
        T CopyFrom(T other);
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

            var r = src.GetRect();
            copyComponent.AnchorMin(r.anchorMin);
            copyComponent.AnchorMax(r.anchorMax);
            copyComponent.Pos(src.GetPos3D());
            copyComponent.Pivot(r.pivot);
            copyComponent.OffsetMin(r.offsetMin);
            copyComponent.OffsetMax(r.offsetMax);
            copyComponent.LocalScale(r.localScale);
            copyComponent.GetRect().localRotation = r.localRotation;
            copyComponent.Size(src.GetSize());
            return copyComponent;
        }
    }
}