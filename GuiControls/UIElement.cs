using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace GuiControls
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UIElement
    {
        [JsonProperty] public string ParentName { get; private set; }
        [JsonProperty] public string Name { get; }
        [JsonProperty] public Point Position { get; set; }
        [JsonProperty] public Point Size { get; set; }
        [JsonProperty] public UIElements Children { get; }
        [JsonProperty] public string FontName { get; }

        public string Text { get; set; }

        public int Width => Size.X;
        public int Height => Size.Y;
        public Rectangle Area => new Rectangle(Position, Size);
        public int Left => Area.Left;
        public int Right => Area.Right;
        public int Top => Area.Top;
        public int Bottom => Area.Bottom;
        public Point TopLeft => new Point(Area.Left, Area.Top);
        public Point TopRight => new Point(Area.Right, Area.Top);
        public Point BottomLeft => new Point(Area.Left, Area.Bottom);
        public Point BottomRight => new Point(Area.Right, Area.Bottom);
        public Point Center => new Point(Area.Center.X, Area.Center.Y);

        [JsonConstructor]
        protected UIElement(string name, string fontName, Point position, Point size, params UIElement[] children)
        {
            Name = name;
            FontName = fontName;
            Position = position;
            Size = size;

            Children = UIElements.Create();
            foreach (var item in children)
            {
                item.ParentName = name;
                Children.Add(item);
            }
        }

        public static UIElement Create(string name, string fontName, Point position, Point size, params UIElement[] children)
        {
            return new UIElement(name, fontName, position, size, children);
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw()
        {
            foreach (UIElement item in Children)
            {
                item.Draw();
            }
        }

        public static UIElement Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<UIElement>(json);
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}