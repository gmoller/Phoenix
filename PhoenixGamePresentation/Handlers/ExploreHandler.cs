using PhoenixGameLibrary;
using PhoenixGamePresentation.Views;
using Utilities.ExtensionMethods;

namespace PhoenixGamePresentation.Handlers
{
    internal static class ExploreHandler
    {
        internal static bool MustFindNewExploreLocation(object sender, float deltaTime)
        {
            var stackView = (StackView)sender;

            var mustFindNewExploreLocation = stackView.Status == UnitStatus.Explore && stackView.HasNoMovementPath();

            return mustFindNewExploreLocation;
        }

        internal static void SetMovementPathToNewExploreLocation(object sender, ActionArgs e)
        {
            var stackView = (StackView)sender;

            // find closest unexplored cell
            var cellGrid = stackView.WorldView.CellGrid;
            var cell = cellGrid.GetClosestUnexploredCell(stackView.Location);

            if (cell != Cell.Empty)
            {
                // find best path to unexplored cell
                var path = MovementPathDeterminer.DetermineMovementPath(stackView, stackView.Location, cell.ToPoint, cellGrid);

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