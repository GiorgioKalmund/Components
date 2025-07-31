using UnityEngine;
using UnityEngine.Events;

namespace Components.Runtime.Components
{
    public class PopupComponent : ButtonComponent, IRenderable
    {
        public bool open = false;

        public UnityEvent onPopupOpen;
        public UnityEvent onPopupClose;

        private Canvas _canvas;
        private Color _defaultBackdropColor = UnityEngine.Color.black;

        public override void Awake()
        {
            base.Awake();
            Function(ClosePopup);

            onPopupOpen ??= new UnityEvent();
            onPopupClose ??= new UnityEvent();
        }
    
        public override void Start()
        {
            base.Start();
            DisplayName = "PopupComponent";
            
            this.FullScreen(_canvas);
            
            if (open) OpenPopup(); else ClosePopup();
        }

        protected PopupComponent DontCloseOnStart()
        {
            open = true;
            return this;
        }

        public void Render() { }

        public PopupComponent Build(Canvas canvas, float alpha = 0.6f, Color? backdropColor = null)
        {
            _canvas = canvas;
            Color c;
            if (backdropColor.HasValue)
                c = backdropColor.Value;
            else
                c = _defaultBackdropColor;
            
            Color(c);
            DisabledColor(c);
            Alpha(alpha);
            return this;
        }

        public virtual void OpenPopup()
        {
            GetRect().localScale = Vector2.one;

            open = true;
            this.BringToFront();
            Render();

            onPopupOpen?.Invoke();
        }

        public virtual void ClosePopup()
        {
            this.LocalScale(Vector2.zero);
            open = false;

            onPopupClose?.Invoke();
        }

        public bool IsOpen()
        {
            return open;
        }

        public PopupComponent DontCloseOnBackGroundTap(bool dontClose = true)
        {
            if (dontClose)
                this.Lock();
            else 
                this.Unlock();
            return this;
        }

        public PopupComponent AddContent(BaseComponent component)
        {
            component.Parent(this);
            return this;
        }
    }
}