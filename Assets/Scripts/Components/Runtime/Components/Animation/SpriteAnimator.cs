using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Components.Runtime.Components.Animation
{
    [RequireComponent(typeof(ImageComponent), typeof(Image))]
    public class SpriteAnimator : MonoBehaviour
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
        public bool NativeSizing  {get; private set;  }
        public Vector2 NativeSizeFactor { get; private set; }
        
        public ImageComponent Image { get; private set; }

        public void Play() => CurrentState = State.Running;
        public void Pause() => CurrentState = State.Paused;

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
                        CurrentState = State.Completed;
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
            
            if (NativeSizing)
                Image.NativeSize(NativeSizeFactor);
        }

        float GetFrameTime()
        {
            return 1f / (CurrentAnimation.FramesPerSecond[CurrentFrame] * Speed);
        }

        public SpriteAnimator UseNativeSizing(float scaleFactorX, float scaleFactorY)
        {
            NativeSizing = true;
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
    }
}