﻿using GameLogic;

namespace PhoenixGameLibrary.Commands
{
    public class EndTurnCommand : Command
    {
        internal override void Execute()
        {
            Globals.Instance.World.EndTurn();
        }
    }
}