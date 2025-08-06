using UnityEngine;

namespace Components.Runtime.Components
{
    public interface IFocusable
    {

        private static IFocusable _focusedElement;
        public static IFocusable FocusedElement
        {
            get => _focusedElement;
            set
            {
                _focusedElement = value;
            }
        }

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

        public static void ClearFocus()
        {
            FocusedElement.UnFocus();
        }
    }

    public static class FocusableHelper
    {
        public static void Focus<T>(this T focusable) where T : IFocusable
        {
            IFocusable.FocusedElement?.UnFocus();
            IFocusable.FocusedElement = focusable;
            focusable?.HandleFocus();
        }
        public static void UnFocus<T>(this T focusable) where T : IFocusable
        {
            IFocusable.SetLastFocusedElement(focusable);
            focusable?.HandleUnfocus();
            IFocusable.FocusedElement = null;
        }

        public static bool IsFocused<T>(this T focusable) where T : IFocusable
        {
            return IFocusable.FocusedElement is T f && f.Equals(focusable);
        }
    }
}