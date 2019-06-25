using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using AssetsLibrary;
using Utilities;

namespace PhoenixGameLibrary
{
    public class Settlements
    {
        private readonly Camera _camera;

        private List<Settlement> _settlements;

        public Settlements(Camera camera)
        {
            _camera = camera;
            _settlements = new List<Settlement>();
        }

        public void LoadContent(ContentManager content)
        {
            AssetsManager.Instance.AddTexture("VillageSmall00", "Textures\\villageSmall00");
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            foreach (var settlement in _settlements)
            {
                settlement.Update(gameTime, input);
            }
        }

        public void Draw()
        {
            DeviceManager.Instance.SetViewport(DeviceManager.Instance.MapViewport);

            foreach (var settlement in _settlements)
            {
                settlement.Draw();
            }

            DeviceManager.Instance.ResetViewport();
        }

        public void AddSettlement(ContentManager content)
        {
            var settlement = new Settlement("Fairhaven", new Point(6, 6), 4, _camera);
            settlement.LoadContent(content);
            _settlements.Add(settlement);
        }
    }
}