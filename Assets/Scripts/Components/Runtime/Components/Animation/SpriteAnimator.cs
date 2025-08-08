using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Components.Runtime.Components.Animation
{
    [RequireComponent(typeof(ImageComponent), typeof(Image))]
    public class SpriteAnimator : BaseComponent, ICopyable<SpriteAnimator>
    {
        public enum State
        {
            None,
            Running,
            Paused,
            Completed
        }

        public enum Type
        {
            Loop,
            Once,
            PingPong
        }

        private enum PingPongPhase
        {
            Ping,
            Pong
        }
        
        public SpriteAnimation CurrentAnimation;
        public State CurrentState { get; private set; } = State.None;
        public Type AnimationType { get; private set;  }
        private PingPongPhase _currentPingPongPhase = PingPongPhase.Ping;
        public float Speed { get; private set; } = 1f;
        public float ElapsedTime { get; private set; }
        public int CurrentFrame { get; private set; }
        public int AnimationLength => CurrentAnimation.Frames.Length;
        
        // Sprite Sizing
        protected bool UseNativeSizing  {get; private set;  }
        protected Vector2 NativeSizeFactor { get; private set; }
        
        public ImageComponent Image { get; private set; }

        public void Play()
        {
            if (CurrentState == State.Completed)
                return;
            
            CurrentState = State.Running;
        }

        public void Pause()
        {
            if (CurrentState == State.Completed)
                return;
            
            CurrentState = State.Paused;
        }

        private void Start()
        {
            Image = GetComponent<ImageComponent>();
            SetFrame();
        }

        private void Update()
        {
            if (CurrentState != State.Running || CurrentAnimation == null)
                return;
            
            ElapsedTime += Time.deltaTime;
            
            float frameDuration = GetFrameTime();
            if (ElapsedTime >= frameDuration)
            {
                NextFrame();
            }
        }

        public SpriteAnimator CreateAnimation(SpriteAnimation anim, Type animationType, float speed = 1f)
        {
            CurrentAnimation = anim;
            AnimationType = animationType;
            Speed = speed;
            return this;
        }

        public void NextFrame()
        {
            switch (AnimationType)
            {
                case Type.Loop:
                {
                    CurrentFrame++;
                    CurrentFrame %= AnimationLength;
                    SetFrame();
                    break;   
                }
                case Type.Once:
                {
                    CurrentFrame++;
                    if (CurrentFrame < AnimationLength)
                        SetFrame();
                    else
                    {
                        CurrentFrame = AnimationLength - 1;
                        CurrentState = State.Completed;
                    }
                    break;
                }
                case Type.PingPong:
                {
                    if (_currentPingPongPhase == PingPongPhase.Ping)
                    {
                        if (CurrentFrame == AnimationLength - 1)
                        {
                            _currentPingPongPhase = PingPongPhase.Pong;
                            CurrentFrame--;
                            SetFrame();
                            break;
                        }
                        CurrentFrame++;
                        SetFrame();
                    }
                    else
                    {
                        if (CurrentFrame == 0)
                        {
                            _currentPingPongPhase = PingPongPhase.Ping;
                            CurrentFrame++;
                            SetFrame();
                            break;
                        }
                        CurrentFrame--;
                        SetFrame();
                    }
                    break;
                }
            }
            
            ElapsedTime = 0;
        }

        public void ResetAnimation()
        {
            CurrentFrame = 0;
            CurrentState = State.None;
            ElapsedTime = 0;
            _currentPingPongPhase = PingPongPhase.Ping;
            SetFrame();
        }

        public void Clear()
        {
            ResetAnimation();
            CurrentAnimation = null;
        }
        
        public void RestartAnimation()
        {
            ResetAnimation();
            Play();
        }

        private void SetFrame()
        {
            Image.Sprite(CurrentAnimation.Frames[CurrentFrame]);
            
            if (UseNativeSizing)
                Image.NativeSize(NativeSizeFactor);
        }

        float GetFrameTime()
        {
            if (CurrentFrame < 0 || CurrentFrame >= CurrentAnimation.FramesPerSecond.Length)
                Debug.Log(AnimationType + ": "+ CurrentAnimation.FramesPerSecond.Length + $">> ({CurrentFrame})");
            return 1f / (CurrentAnimation.FramesPerSecond[CurrentFrame] * Speed);
        }

        public SpriteAnimator NativeSizing(float scaleFactorX, float scaleFactorY)
        {
            UseNativeSizing = true;
            NativeSizeFactor = new Vector2(scaleFactorX, scaleFactorY);
            return this;
        }

        public SpriteAnimator Configure(Type? type = null, float? speed = null)
        {
            if (type.HasValue)
                AnimationType = type.Value;
            if (speed.HasValue)
                Speed = speed.Value;
            return this;
        }

        public SpriteAnimator Copy(bool fullyCopyRect = true)
        {
            SpriteAnimator copyAnimator = this.BaseCopy(this);
            return copyAnimator.CopyFrom(this, fullyCopyRect);
        }

        public SpriteAnimator CopyFrom(SpriteAnimator other, bool fullyCopyRect = true)
        {
            DisplayName = other.DisplayName + " (Copy)";
            CopyRect(other.GetRect(), this, fullyCopyRect);
            
            NativeSizeFactor = other.NativeSizeFactor;
            UseNativeSizing = other.UseNativeSizing;
            Speed = other.Speed;
            CurrentAnimation = other.CurrentAnimation;
            CurrentState = other.CurrentState;
            CurrentFrame = other.CurrentFrame;

            return this;
        }

        private void OnDrawGizmos()
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = UnityEngine.Color.red;
            style.fontSize = 14; 
            
            Handles.Label(transform.position, $"Type: {AnimationType}\nState: {CurrentState}\nFrame: {CurrentFrame}\nSpeed: {Speed}\nFrames: {CurrentAnimation.Frames.Length}", style);
        }
    }
}