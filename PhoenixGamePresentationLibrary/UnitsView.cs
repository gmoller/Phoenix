﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.GameData;

namespace PhoenixGamePresentationLibrary
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class UnitsView : IEnumerable<UnitView>
    {
        private readonly WorldView _worldView;
        private ContentManager _content;

        private UnitsStack _unitsStack;
        private Dictionary<Guid, UnitView> _unitViews;

        public float Moves => _unitsStack.GetMoves;
        public List<string> MovementType => _unitsStack.MovementTypeName;

        public UnitsView(WorldView worldView)
        {
            _worldView = worldView;
            _unitViews = new Dictionary<Guid, UnitView>();
        }

        internal void LoadContent(ContentManager content)
        {
            _content = content;
        }

        internal void Refresh(Units units)
        {
            var u = units[0]; // TODO: fix this
            var cell = Globals.Instance.World.OverlandMap.CellGrid.GetCell(u.Location);
            var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
            _unitsStack = new UnitsStack(units, terrainType);

            _unitViews = new Dictionary<Guid, UnitView>();
            foreach (var unit in units)
            {
                var unitView = new UnitView(_worldView, unit);
                unitView.LoadContent(_content); // TODO: get rid of this
                _unitViews.Add(unit.Id, unitView);
            }
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            foreach (var unitView in _unitViews.Values)
            {
                unitView.Update(input, deltaTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var unit in _unitViews)
            {
                unit.Value.Draw(spriteBatch);
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