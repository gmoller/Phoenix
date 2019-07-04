using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AssetsLibrary
{
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
    }

    public class AtlasFrames
    {
        private List<AtlasFrame> _frames;

        public AtlasFrames()
        {
            _frames = new List<AtlasFrame>();
        }

        public AtlasFrame this[int index] => _frames[index];

        public AtlasFrame this[string name]
        {
            get
            {
                var frame = _frames.Find(o => o.Name == name);

                return frame;
            }
        }

        public void Add(AtlasFrame frame)
        {
            _frames.Add(frame);
        }
    }
}