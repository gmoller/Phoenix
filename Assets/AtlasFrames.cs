using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assets
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class AtlasFrames : IEnumerable<AtlasFrame>
    {
        private readonly string _atlasName;

        private readonly List<AtlasFrame> _framesList;
        private readonly Dictionary<string, int> _framesDictionary;

        public AtlasFrames(string atlasName)
        {
            _atlasName = atlasName;
            _framesList = new List<AtlasFrame>();
            _framesDictionary = new Dictionary<string, int>();
        }

        public AtlasFrame this[int index] => _framesList[index];

        public AtlasFrame this[string name]
        {
            get
            {
                try
                {
                    var frameIndex = _framesDictionary[name];

                    return _framesList[frameIndex];
                }
                catch (KeyNotFoundException e)
                {
                    throw new Exception($"Frame [{name}] not found in atlas [{_atlasName}].", e);
                }
            }
        }

        public void Add(AtlasFrame frame)
        {
            var frameIndex = _framesList.Count;
            _framesList.Add(frame);
            _framesDictionary.Add(frame.Name, frameIndex);
        }

        public IEnumerator<AtlasFrame> GetEnumerator()
        {
            foreach (var item in _framesList)
            {
                yield return item;
            }
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string DebuggerDisplay => $"{{Name={_atlasName},Count={_framesList.Count}}}";
    }
}