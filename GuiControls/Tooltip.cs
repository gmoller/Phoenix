using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Utilities;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Tooltip : Control
    {
        #region State
        #endregion

        public Tooltip(Vector2 position, Alignment positionAlignment, Vector2 size, string textureAtlas, string textureName, int topPadding, int bottomPadding, int leftPadding, int rightPadding, string name, float layerDepth = 0.0f) : base(position, positionAlignment, size, textureAtlas, textureName, null, null, null, null, name, layerDepth)
        {
            var frame = new Frame(position, positionAlignment, size, textureAtlas, textureName, topPadding, bottomPadding, leftPadding, rightPadding, "frame");
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

        protected Tooltip(Tooltip copyThis) : base(copyThis)
        {
        }

        public override IControl Clone() { return new Tooltip(this); }

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
            this["frame"].SetTopLeftPosition(point);
        }
    }
}