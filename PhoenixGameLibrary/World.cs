using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HexLibrary;
using Utilities;

namespace PhoenixGameLibrary
{
    public class World
    {
        private readonly Camera _camera;
        private readonly OverlandMap _overlandMap;
        private readonly Settlements _settlements;

        public World()
        {
            _camera = new Camera(new Viewport(0, 0, 1500, 755));
            _overlandMap = new OverlandMap(_camera);
            _settlements = new Settlements(_camera);
        }

        public void LoadContent(ContentManager content)
        {
            _overlandMap.LoadContent(content);
            _settlements.LoadContent(content);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _camera.UpdateCamera(gameTime, input);
            _overlandMap.Update(gameTime, input);

            var worldPos = _camera.ScreenToWorld(new Vector2(input.MousePostion.X - 10, input.MousePostion.Y - 10));
            DeviceManager.Instance.WorldPosition = new Point((int)worldPos.X, (int)worldPos.Y);
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel((int)worldPos.X, (int)worldPos.Y);
            DeviceManager.Instance.WorldHex = new Point(worldHex.Col, worldHex.Row);
        }

        public void Draw()
        {
            _overlandMap.Draw();
            _settlements.Draw();
        }

        //public static Vector2 CalculateWorldPosition(int colQ, int rowR)
        //{
        //    // odd-r horizontal layout
        //    float x;
        //    if (rowR % 2 == 0)
        //    {
        //        x = Constants.HEX_WIDTH * colQ;
        //    }
        //    else
        //    {
        //        x = Constants.HEX_WIDTH * colQ + Constants.HEX_HALF_WIDTH;
        //    }
        //    float y = Constants.HEX_THREE_QUARTER_HEIGHT * rowR;

        //    var position = new Vector2(x, y);

        //    return position;
        //}
    }
}