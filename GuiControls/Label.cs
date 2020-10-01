using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Assets;
using Input;
using MonoGameUtilities;
using MonoGameUtilities.ExtensionMethods;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public abstract class Label : Control
    {
        #region State
        private Func<string> GetTextFunc { get; }

        public string Text { get; set; }
        private string FontName { get; }
        private Color TextColor { get; }
        private Color? TextShadowColor { get; }
        private Color? BackColor { get; }
        private Color? BorderColor { get; }
        private float Scale { get; }

        private SpriteFont Font { get; set; }
        #endregion

        protected Label(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            string text,
            Func<string> getTextFunc,
            string fontName,
            Color textColor,
            string name,
            Color? textShadowColor = null,
            Color? backColor = null,
            Color? borderColor = null,
            float scale = 1.0f,
            float layerDepth = 0.0f) : 
            base(
                position,
                positionAlignment,
                size,
                name,
                layerDepth)
        {
            Text = text;
            GetTextFunc = getTextFunc;
            FontName = fontName;
            TextColor = textColor;
            TextShadowColor = textShadowColor;
            BackColor = backColor;
            BorderColor = borderColor;
            Scale = scale;
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            Font = AssetsManager.Instance.GetSpriteFont(FontName);
        }

        protected abstract Vector2 DetermineOffset(SpriteFont font, Vector2 size, string text, float scale);

        public override void Update(InputHandler input, float deltaTime, Viewport? viewport)
        {
            if (GetTextFunc != null)
            {
                Text = GetTextFunc();
            }

            base.Update(input, deltaTime, viewport);
        }

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            if (BackColor != null)
            {
                spriteBatch.FillRectangle(
                    ActualDestinationRectangle, 
                    BackColor.Value, 
                    LayerDepth);
            }

            var offset = DetermineOffset(Font, Size.ToVector2(), Text, Scale);
            if (TextShadowColor != null)
            {
                spriteBatch.DrawString(
                    Font, 
                    Text, 
                    TopLeft.ToVector2() + offset + Vector2.One, 
                    TextShadowColor.Value, 
                    0.0f, 
                    Vector2.Zero,
                    Scale, 
                    SpriteEffects.None, 
                    LayerDepth);
            }

            if (BorderColor != null)
            {
                spriteBatch.DrawRectangle(
                    new Rectangle(
                        ActualDestinationRectangle.X,
                        ActualDestinationRectangle.Y,
                        ActualDestinationRectangle.Width - 1,
                        ActualDestinationRectangle.Height - 1),
                    BorderColor.Value, 
                    LayerDepth);
            }

            spriteBatch.DrawString(
                Font, 
                Text, 
                TopLeft.ToVector2() + offset, 
                TextColor, 
                0.0f, 
                Vector2.Zero,
                Scale, 
                SpriteEffects.None, 
                LayerDepth);
        }

        public void SetText(string text)
        {
            Text = text;
        }
    }

    //[DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    //public class LabelAutoSized : Label
    //{
    //    #region State
    //    private readonly Vector2 _position;
    //    private readonly Alignment _positionAlignment;
    //    private readonly bool _autoSize;

    //    public override string Text
    //    {
    //        get => base.Text;
    //        set
    //        {
    //            base.Text = value;
    //            Resize(value);
    //        }
    //    }
    //    #endregion

    //    public LabelAutoSized(
    //        Vector2 position,
    //        Alignment positionAlignment,
    //        Func<string> getTextFunc,
    //        string fontName,
    //        Color textColor,
    //        string name,
    //        IControl parent = null) :
    //        this(
    //            position,
    //            positionAlignment,
    //            null,
    //            getTextFunc,
    //            fontName,
    //            textColor,
    //            name,
    //            null,
    //            null,
    //            null,
    //            0.0f,
    //            parent)
    //    {
    //    }

    //    public LabelAutoSized(
    //        Vector2 position,
    //        Alignment positionAlignment,
    //        Func<string> getTextFunc,
    //        string fontName,
    //        Color textColor,
    //        string name,
    //        Color? textShadowColor,
    //        IControl parent = null) :
    //        this(
    //            position,
    //            positionAlignment,
    //            null,
    //            getTextFunc,
    //            fontName,
    //            textColor,
    //            name,
    //            textShadowColor,
    //            null,
    //            null,
    //            0.0f,
    //            parent)
    //    {
    //    }

    //    public LabelAutoSized(
    //        Vector2 position,
    //        Alignment positionAlignment,
    //        string text,
    //        string fontName,
    //        Color textColor,
    //        string name,
    //        Color? textShadowColor,
    //        IControl parent = null) :
    //        this(
    //            position,
    //            positionAlignment,
    //            text,
    //            null,
    //            fontName,
    //            textColor,
    //            name,
    //            textShadowColor,
    //            null,
    //            null,
    //            0.0f,
    //            parent)
    //    {
    //    }

    //    public LabelAutoSized(
    //        Vector2 position,
    //        Alignment positionAlignment,
    //        string text,
    //        string fontName,
    //        Color textColor,
    //        string name,
    //        IControl parent) :
    //        this(
    //            position,
    //            positionAlignment,
    //            text,
    //            null,
    //            fontName,
    //            textColor,
    //            name,
    //            null,
    //            null,
    //            null,
    //            0.0f,
    //            parent)
    //    {
    //    }

    //    public LabelAutoSized(
    //        Vector2 position,
    //        Alignment positionAlignment,
    //        string text,
    //        string fontName,
    //        Color textColor,
    //        string name,
    //        Color? textShadowColor = null) :
    //        this(
    //            position,
    //            positionAlignment,
    //            text,
    //            null,
    //            fontName,
    //            textColor,
    //            name,
    //            textShadowColor)
    //    {
    //    }

    //    private LabelAutoSized(
    //        Vector2 position,
    //        Alignment positionAlignment,
    //        string text,
    //        Func<string> getTextFunc,
    //        string fontName,
    //        Color textColor,
    //        string name,
    //        Color? textShadowColor = null,
    //        Color? backColor = null,
    //        Color? borderColor = null,
    //        float layerDepth = 0.0f,
    //        IControl parent = null) :
    //        base(
    //            position,
    //            positionAlignment,
    //            Vector2.Zero,
    //            text,
    //            getTextFunc,
    //            fontName,
    //            textColor,
    //            name,
    //            textShadowColor,
    //            backColor,
    //            borderColor,
    //            layerDepth,
    //            parent)
    //    {
    //        _position = position;
    //        _positionAlignment = positionAlignment;
    //        _autoSize = true;
    //    }

    //    public override void LoadContent(ContentManager content)
    //    {
    //        base.LoadContent(content);
    //        Resize(Text);
    //    }

    //    protected override Vector2 DetermineOffset(SpriteFont font, Vector2 size, string text)
    //    {
    //        return Vector2.Zero;
    //    }

    //    private void Resize(string text)
    //    {
    //        if (_autoSize && Font != null && text != null)
    //        {
    //            var size = Font.MeasureString(text);
    //            DetermineArea(_position, _positionAlignment, size);
    //        }
    //    }
    //}

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class LabelSized : Label
    {
        #region State
        private Alignment ContentAlignment { get; }
        #endregion

        /// <summary>
        /// Use this constructor if Label is to be used as a child of another control.
        /// When a control is a child of another control, it's position will be relative
        /// to the parent control. Therefore there is no need to pass in a position.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="contentAlignment"></param>
        /// <param name="text"></param>
        /// <param name="fontName"></param>
        /// <param name="textColor"></param>
        /// <param name="textShadowColor"></param>
        /// <param name="backColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="scale"></param>
        public LabelSized(
            string name,
            Vector2 size,
            Alignment contentAlignment,
            string text,
            string fontName,
            Color textColor,
            Color? textShadowColor = null,
            Color? backColor = null,
            Color? borderColor = null,
            float scale = 1.0f) :
            this(
                Vector2.Zero, 
                Alignment.TopLeft,
                size,
                contentAlignment,
                text,
                null,
                fontName,
                textColor,
                name,
                textShadowColor,
                backColor,
                borderColor,
                scale)
        {
        }

        /// <summary>
        /// Use this constructor if Label is to be used as a child of another control.
        /// When a control is a child of another control, it's position will be relative
        /// to the parent control. Therefore there is no need to pass in a position.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="contentAlignment"></param>
        /// <param name="getTextFunc"></param>
        /// <param name="fontName"></param>
        /// <param name="textColor"></param>
        /// <param name="textShadowColor"></param>
        /// <param name="backColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="scale"></param>
        public LabelSized(
            string name,
            Vector2 size,
            Alignment contentAlignment,
            Func<string> getTextFunc,
            string fontName,
            Color textColor,
            Color? textShadowColor = null,
            Color? backColor = null,
            Color? borderColor = null,
            float scale = 1.0f) :
            this(
                Vector2.Zero,
                Alignment.TopLeft,
                size,
                contentAlignment,
                string.Empty,
                getTextFunc,
                fontName,
                textColor,
                name,
                textShadowColor,
                backColor,
                borderColor,
                scale)
        {
        }

        /// <summary>
        /// Use this constructor if Label is expected to be stand alone (have no parent).
        /// </summary>
        /// <param name="position"></param>
        /// <param name="positionAlignment"></param>
        /// <param name="size"></param>
        /// <param name="contentAlignment"></param>
        /// <param name="text"></param>
        /// <param name="fontName"></param>
        /// <param name="textColor"></param>
        /// <param name="name"></param>
        /// <param name="textShadowColor"></param>
        /// <param name="backColor"></param>
        /// <param name="borderColor"></param>
        /// <param name="scale"></param>
        public LabelSized(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            Alignment contentAlignment,
            string text,
            string fontName,
            Color textColor,
            string name,
            Color? textShadowColor = null,
            Color? backColor = null,
            Color? borderColor = null,
            float scale = 1.0f) :
            this(
                position,
                positionAlignment,
                size,
                contentAlignment,
                text,
                null,
                fontName,
                textColor,
                name,
                textShadowColor,
                backColor,
                borderColor,
                scale)
        {
        }

        /// <summary>
        /// Use this constructor if Label is expected to be stand alone (have no parent).
        /// </summary>
        /// <param name="position"></param>
        /// <param name="positionAlignment"></param>
        /// <param name="size"></param>
        /// <param name="contentAlignment"></param>
        /// <param name="text"></param>
        /// <param name="fontName"></param>
        /// <param name="textColor"></param>
        /// <param name="name"></param>
        /// <param name="textShadowColor"></param>
        /// <param name="scale"></param>
        public LabelSized(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            Alignment contentAlignment,
            string text,
            string fontName,
            Color textColor,
            string name,
            Color? textShadowColor = null,
            float scale = 1.0f) :
            this(
                position,
                positionAlignment,
                size,
                contentAlignment,
                text,
                null,
                fontName,
                textColor,
                name,
                textShadowColor,
                null,
                null,
                scale)
        {
        }

        private LabelSized(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            Alignment contentAlignment,
            string text,
            Func<string> getTextFunc,
            string fontName,
            Color textColor,
            string name,
            Color? textShadowColor = null,
            Color? backColor = null,
            Color? borderColor = null,
            float scale = 1.0f,
            float layerDepth = 0.0f) :
            base(
                position,
                positionAlignment,
                size,
                text,
                getTextFunc,
                fontName,
                textColor,
                name,
                textShadowColor,
                backColor,
                borderColor,
                scale,
                layerDepth)
        {
            ContentAlignment = contentAlignment;
        }

        protected override Vector2 DetermineOffset(SpriteFont font, Vector2 size, string text, float scale)
        {
            var textSize = font.MeasureString(text) * scale;

            return ContentAlignment switch
            {
                Alignment.TopLeft => Vector2.Zero,
                Alignment.TopCenter => new Vector2((size.X - textSize.X) * 0.5f, 0.0f),
                Alignment.TopRight => new Vector2(size.X - textSize.X, 0.0f),
                Alignment.MiddleLeft => new Vector2(0.0f, (size.Y - textSize.Y) * 0.5f),
                Alignment.MiddleCenter => new Vector2((size.X - textSize.X) * 0.5f, (size.Y - textSize.Y) * 0.5f),
                Alignment.MiddleRight => new Vector2(size.X - textSize.X, (size.Y - textSize.Y) * 0.5f),
                Alignment.BottomLeft => new Vector2(0.0f, size.Y - textSize.Y),
                Alignment.BottomCenter => new Vector2((size.X - textSize.X) * 0.5f, size.Y - textSize.Y),
                Alignment.BottomRight => new Vector2(size.X - textSize.X, size.Y - textSize.Y),
                _ => throw new Exception($"ContentAlignment [{ContentAlignment}] is not implemented."),
            };
        }
    }
}