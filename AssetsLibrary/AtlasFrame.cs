using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace AssetsLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct AtlasFrame
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Rectangle ToRectangle()
        {
            return new Rectangle(X, Y, Width, Height);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Name={Name}}}";
    }

    public class AtlasFrames
    {
        private readonly string _atlasName;
        private readonly List<AtlasFrame> _frames;

        public AtlasFrames(string atlasName)
        {
            _atlasName = atlasName;
            _frames = new List<AtlasFrame>();
        }

        public AtlasFrame this[int index] => _frames[index];

        public AtlasFrame this[string name]
        {
            get
            {
                var frame = _frames.Find(o => o.Name == name);

                if (frame.Width == 0 && frame.Height == 0) throw new Exception($"Frame [{name}] not found in atlas [{_atlasName}].");

                return frame;
            }
        }

        public void Add(AtlasFrame frame)
        {
            _frames.Add(frame);
        }
    }
}