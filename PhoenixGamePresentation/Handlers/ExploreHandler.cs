using PhoenixGameLibrary;
using PhoenixGamePresentation.Views;
using Utilities.ExtensionMethods;

namespace PhoenixGamePresentation.Handlers
{
    internal static class ExploreHandler
    {
        internal static bool MustFindNewExploreLocation(StackView stackView)
        {
            var mustFindNewExploreLocation = stackView.Status == UnitStatus.Explore && stackView.HasNoMovementPath();

            return mustFindNewExploreLocation;
        }

        internal static void SetMovementPathToNewExploreLocation(StackView stackView, World world)
        {
            // find closest unexplored cell
            var cell = world.OverlandMap.CellGrid.GetClosestUnexploredCell(stackView.Location);

            if (cell != Cell.Empty)
            {
                // find best path to unexplored cell
                var path = MovementPathDeterminer.DetermineMovementPath(stackView, stackView.Location, cell.ToPoint, world);

                if (path.Count > 0)
                {
                    path = path.RemoveLast(stackView.SightRange);
                    stackView.SetMovementPath(path);
                }
                else
                {
                    // no location found to explore
                    stackView.SetStatusToNone();
                }
            }
            else
            {
                // all locations explored
                stackView.SetStatusToNone();
            }
        }
    }
}