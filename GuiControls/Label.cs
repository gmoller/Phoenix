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
        private readonly Func<string> _getTextFunc;

        protected readonly string FontName;
        protected readonly Color TextColor;
        protected readonly Color? TextShadowColor;
        protected readonly Color? BackColor;
        protected readonly Color? BorderColor;

        protected SpriteFont Font { get; set; }
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
            float layerDepth = 0.0f) : 
            base(
                position, 
                positionAlignment, 
                size, 
                string.Empty, 
                string.Empty, 
                null, 
                null, 
                null, 
                null,
                name,
                layerDepth)
        {
            Text = text;
            _getTextFunc = getTextFunc;
            FontName = fontName;
            TextColor = textColor;
            TextShadowColor = textShadowColor;
            BackColor = backColor;
            BorderColor = borderColor;
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            Font = AssetsManager.Instance.GetSpriteFont(FontName);
        }

        protected abstract Vector2 DetermineOffset(SpriteFont font, Vector2 size, string text);

        public override void Update(InputHandler input, float deltaTime, Viewport? viewport)
        {
            if (_getTextFunc != null)
            {
                Text = _getTextFunc();
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

            var offset = DetermineOffset(Font, Size.ToVector2(), Text);
            if (TextShadowColor != null)
            {
                spriteBatch.DrawString(
                    Font, 
                    Text, 
                    TopLeft.ToVector2() + offset + Vector2.One, 
                    TextShadowColor.Value, 
                    0.0f, 
                    Vector2.Zero, 
                    1.0f, 
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
                1.0f, 
                SpriteEffects.None, 
                LayerDepth);
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
        private readonly Alignment _contentAlignment;
        #endregion State

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
        public LabelSized(
            string name,
            Vector2 size,
            Alignment contentAlignment,
            string text,
            string fontName,
            Color textColor,
            Color? textShadowColor = null,
            Color? backColor = null,
            Color? borderColor = null) :
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
                borderColor)
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
        public LabelSized(
            string name,
            Vector2 size,
            Alignment contentAlignment,
            Func<string> getTextFunc,
            string fontName,
            Color textColor,
            Color? textShadowColor = null,
            Color? backColor = null,
            Color? borderColor = null) :
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
                borderColor)
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
            Color? borderColor = null) :
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
                borderColor)
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
        public LabelSized(
            Vector2 position,
            Alignment positionAlignment,
            Vector2 size,
            Alignment contentAlignment,
            string text,
            string fontName,
            Color textColor,
            string name,
            Color? textShadowColor = null) :
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
                textShadowColor)
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
                layerDepth)
        {
            _contentAlignment = contentAlignment;
            ActualDestinationRectangle = DetermineArea(position, positionAlignment, size);
        }

        protected override Vector2 DetermineOffset(SpriteFont font, Vector2 size, string text)
        {
            var textSize = font.MeasureString(text);

            switch (_contentAlignment)
            {
                case Alignment.TopLeft:
                    return Vector2.Zero;
                case Alignment.TopCenter:
                    return new Vector2((size.X - textSize.X) * 0.5f, 0.0f);
                case Alignment.TopRight:
                    return new Vector2(size.X - textSize.X, 0.0f);

                case Alignment.MiddleLeft:
                    return new Vector2(0.0f, (size.Y - textSize.Y) * 0.5f);
                case Alignment.MiddleCenter:
                    return new Vector2((size.X - textSize.X) * 0.5f, (size.Y - textSize.Y) * 0.5f);
                case Alignment.MiddleRight:
                    return new Vector2(size.X - textSize.X, (size.Y - textSize.Y) * 0.5f);

                case Alignment.BottomLeft:
                    return new Vector2(0.0f, size.Y - textSize.Y);
                case Alignment.BottomCenter:
                    return new Vector2((size.X - textSize.X) * 0.5f, size.Y - textSize.Y);
                case Alignment.BottomRight:
                    return new Vector2(size.X - textSize.X, size.Y - textSize.Y);

                default:
                    throw new Exception($"ContentAlignment [{_contentAlignment}] is not implemented.");
            }
        }
    }
}