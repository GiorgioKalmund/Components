using UnityEngine;

namespace Components.Runtime.Components
{
    public interface IFocusable
    {
        
        public static IFocusable FocusedElement;

        private static IFocusable _lastFocusedElement;

        public void HandleFocus();
        public void HandleUnfocus();
        public static void SetLastFocusedElement(IFocusable focusable)
        {
            _lastFocusedElement = focusable;
        }

        public static void FocusLastFocused()
        {
            _lastFocusedElement.Focus();
        }
    }

    public static class FocusableHelper
    {
        public static void Focus<T>(this T focusable) where T : IFocusable
        {
            IFocusable.FocusedElement?.UnFocus();
            
            IFocusable.FocusedElement = focusable;
            focusable.HandleFocus();
        }
        public static void UnFocus<T>(this T focusable) where T : IFocusable
        {
            IFocusable.SetLastFocusedElement(focusable);
            IFocusable.FocusedElement = null;
            focusable.HandleUnfocus();
        }

        public static bool IsFocused<T>(this T focusable) where T : IFocusable
        {
            return IFocusable.FocusedElement is T f && f.Equals(focusable);
        }
    }
}