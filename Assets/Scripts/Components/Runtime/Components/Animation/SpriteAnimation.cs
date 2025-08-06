using System;
using UnityEngine;

namespace Components.Runtime.Components.Animation
{
    public class SpriteAnimation
    {
        public readonly Sprite[] Frames;
        public readonly float[] FramesPerSecond;

        public SpriteAnimation(Sprite[] frames, float[] framesPerSeconds)
        {
            Frames = frames;
            FramesPerSecond = framesPerSeconds;
        }

        public SpriteAnimation(Sprite[] frames, float framesPerSecond)
        {
            Frames = frames;
            FramesPerSecond = new float[frames.Length];
            Array.Fill(FramesPerSecond, framesPerSecond);
        }
    }
}