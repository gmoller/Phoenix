using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GuiControls;

namespace PhoenixGameLibrary.Views
{
    public class HudView
    {
        private UIElement _test;

        public void LoadContent(ContentManager content)
        {
            var element1 = LabelUIElement.Create("Element1", "CrimsonText-Regular-12", new Point(20, 20), new Point(10, 10), Color.Fuchsia);
            var element2 = LabelUIElement.Create("Element2", "CrimsonText-Regular-12", new Point(100, 20), new Point(10, 10), Color.HotPink);

            var hud = UIElement.Create("Hud", "", new Point(0, 0), new Point(100, 100), element1, element2);
            _test = hud;

            var json1 = hud.Serialize();
            hud = UIElement.Deserialize(json1);
            var json2 = hud.Serialize();

            //if (!json1.Equals(json2))
            //{
            //    throw new Exception("Serialization/Deserialization failure!!!");
            //}

            //var all = UIElements.Create();
            //all.Add(hud);
            //var o = all["Hud"];
        }

        public void Update(GameTime gameTime)
        {
            //_test.Text = $"{Guid.NewGuid()}";
            _test.Children["Element1"].Text = "Hello";
            _test.Children["Element2"].Text = "Goodbye";
            _test.Update(gameTime);
        }

        public void Draw()
        {
            _test.Draw();
        }
    }
}