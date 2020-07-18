using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class UnitsView : IEnumerable<UnitView>
    {
        private readonly WorldView _worldView;

        private Texture2D _textures;
        private AtlasSpec2 _atlas;
        private readonly Units _units;
        private readonly Dictionary<Guid, UnitView> _unitViews;

        public UnitsView(WorldView worldView, Units units)
        {
            _worldView = worldView;
            _units = units;
            _unitViews = new Dictionary<Guid, UnitView>();
        }

        internal void LoadContent(ContentManager content)
        {
            _textures = AssetsManager.Instance.GetTexture("Units");
            _atlas = AssetsManager.Instance.GetAtlas("Units");
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            foreach (var unit in _units)
            {
                UnitView unitView;

                if (_unitViews.ContainsKey(unit.Id))
                {
                    unitView = _unitViews[unit.Id];
                }
                else
                {
                    unitView = new UnitView(_worldView, unit);
                    _unitViews.Add(unit.Id, unitView);
                }
                
                unitView.Update(input, deltaTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var unit in _unitViews)
            {
                unit.Value.Draw(spriteBatch, _textures, _atlas);
            }
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_unitViews.Count}}}";

        public IEnumerator<UnitView> GetEnumerator()
        {
            foreach (var item in _unitViews)
            {
                yield return item.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}