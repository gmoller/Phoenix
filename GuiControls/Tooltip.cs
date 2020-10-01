using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Utilities;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Tooltip : ControlWithSingleTexture
    {
        #region State
        private bool IsHoveringOver { get; set; }
        private DateTime TimeStartedHovering { get; set; }
        #endregion

        public Tooltip(Vector2 position, Alignment positionAlignment, Vector2 size, string textureName, int topPadding, int bottomPadding, int leftPadding, int rightPadding, string name, float layerDepth = 0.0f)
            : base(position, positionAlignment, size, textureName, name, layerDepth)
        {
            var frame = new Frame(position, positionAlignment, size, textureName, topPadding, bottomPadding, leftPadding, rightPadding, "frame");
            AddControl(frame);
            frame.AddControl(new Image("background", size - new Vector2(16.0f,16.0f), "TransparentBackground"), Alignment.MiddleCenter, Alignment.MiddleCenter);
            frame.AddControl(new LabelSized("lblId", new Vector2(100.0f, 30.0f), Alignment.TopLeft, "Id:", "Arial-12", Color.Yellow), Alignment.TopLeft, Alignment.TopLeft, new PointI(15, 15));
            frame.AddControl(new LabelSized("lblState", new Vector2(100.0f, 30.0f), Alignment.TopLeft, "State:", "Arial-12", Color.Yellow), Alignment.TopLeft, Alignment.TopLeft, new PointI(15, 45));
            frame.AddControl(new LabelSized("lblStackStatus", new Vector2(100.0f, 30.0f), Alignment.TopLeft, "StackStatus:", "Arial-12", Color.Yellow), Alignment.TopLeft, Alignment.TopLeft, new PointI(15, 75));
            frame.AddControl(new LabelSized("lblIsSelected", new Vector2(100.0f, 30.0f), Alignment.TopLeft, "IsSelected:", "Arial-12", Color.Yellow), Alignment.TopLeft, Alignment.TopLeft, new PointI(15, 105));
            frame.AddControl(new LabelSized("lblOrdersGiven", new Vector2(100.0f, 30.0f), Alignment.TopLeft, "OrdersGiven:", "Arial-12", Color.Yellow), Alignment.TopLeft, Alignment.TopLeft, new PointI(15, 135));
            frame.AddControl(new LabelSized("lblMovementPoints", new Vector2(100.0f, 30.0f), Alignment.TopLeft, "MovementPoints:", "Arial-12", Color.Yellow), Alignment.TopLeft, Alignment.TopLeft, new PointI(15, 165));
            frame.AddControl(new LabelSized("lblCurrent", new Vector2(100.0f, 30.0f), Alignment.TopLeft, "Current:", "Arial-12", Color.Yellow), Alignment.TopLeft, Alignment.TopLeft, new PointI(15, 195));
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            this["frame"].LoadContent(content, loadChildrenContent);
            this["frame.background"].LoadContent(content, loadChildrenContent);
            this["frame.lblId"].LoadContent(content, loadChildrenContent);
            this["frame.lblState"].LoadContent(content, loadChildrenContent);
            this["frame.lblStackStatus"].LoadContent(content, loadChildrenContent);
            this["frame.lblIsSelected"].LoadContent(content, loadChildrenContent);
            this["frame.lblOrdersGiven"].LoadContent(content, loadChildrenContent);
            this["frame.lblMovementPoints"].LoadContent(content, loadChildrenContent);
            this["frame.lblCurrent"].LoadContent(content, loadChildrenContent);
        }

        public override void SetTopLeftPosition(PointI point)
        {
            base.SetTopLeftPosition(point);

            this["frame"].SetTopLeftPosition(point);
        }

        public bool StartHover()
        {
            if (IsHoveringOver)
            {
                return DateTime.Now > TimeStartedHovering.AddMilliseconds(500); // TODO: make configurable
            }

            IsHoveringOver = true;
            TimeStartedHovering = DateTime.Now;

            return false;
        }

        public void StopHover()
        {
            IsHoveringOver = false;
            TimeStartedHovering = DateTime.MinValue;
        }

        public void SetText()
        {
            //this["frame.lblId"].SetText($"Id: {stackView.Id}");
            //this["frame.lblState"].SetText($"State: {stackView.StackViewState}");
            //this["frame.lblStackStatus"].SetText($"StackStatus: {stackView.Stack.Status}");
            //this["frame.lblIsSelected"].SetText($"IsSelected: {stackView.IsSelected}");
            //this["frame.lblOrdersGiven"].SetText($"OrdersGiven: {stackView.OrdersGiven}");
            //this["frame.lblMovementPoints"].SetText($"MovementPoints: {stackView.MovementPoints}");
            //this["frame.lblCurrent"].SetText($"Current: {StackViews.Current}");
        }
    }
}