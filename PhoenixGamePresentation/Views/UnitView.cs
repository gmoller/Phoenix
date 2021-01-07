using System.Diagnostics;
using Microsoft.Xna.Framework;
using PhoenixGameLibrary;

namespace PhoenixGamePresentation.Views
{
    //[DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class UnitView : ViewBase
    {
        internal Unit Unit { get; }
        internal Rectangle DestinationRectangle { get; }

        internal UnitView(Unit unit, Rectangle destinationRectangle)
        {
            Unit = unit;
            DestinationRectangle = destinationRectangle;
        }
    }
}