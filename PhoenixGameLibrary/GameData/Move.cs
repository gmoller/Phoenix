namespace PhoenixGameLibrary.GameData
{
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
    }
}