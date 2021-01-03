using Microsoft.Xna.Framework.Input;
using PhoenixGamePresentation.Events;
using PhoenixGamePresentation.Views;
using Zen.Input;

namespace PhoenixGamePresentation
{
    /// <summary>
    /// This guy is responsible for checking the input state (keyboard and mouse) for this frame
    /// and sending a message to the parties that need to be told.
    /// </summary>
    public static class InputWallah
    {
        internal static void Update(InputHandler input, CursorView cursorView, WorldView worldView, float deltaTime)
        {
            if (worldView.GameStatus == GameStatus.OverlandMap || worldView.GameStatus == GameStatus.CityView)
            {
                if (input.HasMouseMoved)
                {
                    var e = new MouseEventArgs(input.Mouse, null, deltaTime);
                    UpdateCursorPositionEvent.HandleEvent(cursorView, e);
                }
            }

            if (worldView.GameStatus == GameStatus.OverlandMap)
            {
                worldView.CheckIfMouseIsHoveringOverStack(input.Mouse.Location);

                if (input.IsKeyReleased(Keys.Enter))
                {
                    worldView.EndTurn();
                }

                IsKeyReleased(input, worldView, Keys.NumPad1);
                IsKeyReleased(input, worldView, Keys.NumPad2);
                IsKeyReleased(input, worldView, Keys.NumPad3);
                IsKeyReleased(input, worldView, Keys.NumPad4);
                IsKeyReleased(input, worldView, Keys.NumPad6);
                IsKeyReleased(input, worldView, Keys.NumPad7);
                IsKeyReleased(input, worldView, Keys.NumPad8);
                IsKeyReleased(input, worldView, Keys.NumPad9);

                if (input.IsKeyReleased(Keys.C))
                {
                    worldView.FocusCameraOnCurrentlySelectedStackView();
                }

                if (input.IsKeyReleased(Keys.OemTilde))
                {
                    ResetCameraZoomEvent.HandleEvent(worldView.Camera, null);
                }

                if (input.IsLeftMouseButtonReleased)
                {
                    var startUnitMovement  = worldView.CheckForUnitMovementOfCurrentlySelectedStackView(input.Mouse.Location);

                    if (startUnitMovement.startMovement)
                    {
                        worldView.StartUnitMovementOfCurrentlySelectedStackView(startUnitMovement.hexToMoveTo);
                    }
                }

                if (input.IsRightMouseButtonPressed)
                {
                    worldView.SetPotentialMovementOfCurrentlySelectedStackView();
                }

                if (input.IsRightMouseButtonReleased)
                {
                    worldView.ResetPotentialMovementOfCurrentlySelectedStackView();

                    var selectStack = worldView.CheckForSelectionOfStack(input.Mouse.Location);
                    if (selectStack.selectStack)
                    {
                        worldView.SelectStack(selectStack.stackToSelect);
                    }
                    else
                    {
                        var e = new MouseEventArgs(input.Mouse, worldView, deltaTime);
                        OpenSettlementEvent.HandleEvent(null, e);
                    }
                }

                if (input.MouseWheelUp)
                {
                    IncreaseCameraZoomEvent.HandleEvent(worldView.Camera, null);
                }

                if (input.MouseWheelDown)
                {
                    DecreaseCameraZoomEvent.HandleEvent(worldView.Camera, null);
                }

                if (input.IsMouseAtTopOfScreen || input.IsMouseAtBottomOfScreen || input.IsMouseAtLeftOfScreen || input.IsMouseAtRightOfScreen)
                {
                    var e = new MouseEventArgs(input.Mouse, null, deltaTime);
                    MoveCameraEvent.HandleEvent(worldView.Camera, e);
                }

                if (input.IsRightMouseButtonDown && input.HasMouseMoved)
                {
                    var e = new MouseEventArgs(input.Mouse, null, deltaTime);
                    DragCameraEvent.HandleEvent(worldView.Camera, e);
                }
            }
        }

        private static void IsKeyReleased(InputHandler input, WorldView worldView, Keys key)
        {
            if (input.IsKeyReleased(key))
            {
                var startUnitMovement = worldView.CheckForUnitMovementOfCurrentlySelectedStackView(key);

                if (startUnitMovement.startMovement)
                {
                    worldView.StartUnitMovementOfCurrentlySelectedStackView(startUnitMovement.hexToMoveTo);
                }
            }
        }
    }
}