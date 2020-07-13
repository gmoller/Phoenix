using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct Move
    {
        private readonly string _moveType;

        public MoveType MoveType => Globals.Instance.MoveTypes[_moveType];
        public float Moves { get; set; }

        public Move(string moveType, float moves)
        {
            _moveType = moveType;
            Moves = moves;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{MoveType={_moveType},Moves={Moves}}}";
    }

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Moves : IEnumerable<Move>
    {
        private readonly List<Move> _moves;

        public Moves(params Move[] moves)
        {
            _moves = new List<Move>();
            foreach (var item in moves)
            {
                _moves.Add(item);
            }
        }

        public Move this[string moveTypeName]
        {
            get
            {
                foreach (var item in _moves)
                {
                    if (item.MoveType.Name == moveTypeName)
                    {
                        return item;
                    }
                }

                throw new Exception($"Item {moveTypeName} not found in _moves");
            }
        }

        public bool Contains(string moveTypeName)
        {
            foreach (var move in _moves)
            {
                if (move.MoveType.Name == moveTypeName)
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerator<Move> GetEnumerator()
        {
            foreach (var item in _moves)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={_moves.Count}}}";
    }
}