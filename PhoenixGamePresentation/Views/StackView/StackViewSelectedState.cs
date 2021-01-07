using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Zen.Utilities;

namespace PhoenixGamePresentation.Views.StackView
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    internal class StackViewSelectedState : StackViewState
    {
        private const float BLINK_TIME_IN_MILLISECONDS = 250.0f;

        #region State
        private float BlinkCooldownInMilliseconds { get; set; }
        private bool Blink { get; set; }
        #endregion

        internal StackViewSelectedState(StackView stackView)
        {
            StackView = stackView;
            BlinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
            Blink = false;
            StackView.PotentialMovementPath = new List<PointI>();
            StackView.SetAsCurrent(StackView);
            StackView.FocusCameraOn();
            StackView.SetStatusToNone();
        }

        internal override void Update(WorldView worldView, float deltaTime)
        {
            if (CheckForBlinkChange(deltaTime))
            {
                ToggleBlink();
            }

            var restartMovement = CheckForRestartOfMovement();

            if (restartMovement)
            {
                StackView.Move();
            }
        }

        private void ToggleBlink()
        {
            Blink = !Blink;
            BlinkCooldownInMilliseconds = BLINK_TIME_IN_MILLISECONDS;
        }

        private bool CheckForRestartOfMovement()
        {
            if (StackView.HasMovementPath && StackView.MovementPoints > 0.0f)
            {
                return true;
            }

            return false;
        }

        internal override void DrawUnit(SpriteBatch spriteBatch, Camera camera)
        {
            if (!Blink)
            {
                var locationInWorld = camera.WorldHexToWorldPixel(StackView.Stack.LocationHex);
                DrawUnitBackground(spriteBatch);
                DrawUnitIcon(spriteBatch, locationInWorld);
            }
        }

        private bool CheckForBlinkChange(float deltaTime)
        {
            BlinkCooldownInMilliseconds -= deltaTime;
            if (BlinkCooldownInMilliseconds > 0.0f) return false;

            return true;
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => "Selected";
    }
}