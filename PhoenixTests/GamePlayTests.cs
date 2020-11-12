using Microsoft.Xna.Framework.Content;
using NUnit.Framework;
using PhoenixGameLibrary;
using PhoenixGamePresentation;
using Zen.Utilities;

namespace PhoenixTests
{
    [TestFixture]
    public class GamePlayTests
    {
        private PhoenixGame _phoenixGame;
        private PhoenixGameView _phoenixGameView;

        [SetUp]
        public void Setup()
        {
            // Arrange
            var gameMetadata = new GameMetadata();
            var presentationContext = new GlobalContextPresentation();
            CallContext<GameMetadata>.SetData("GameMetadata", gameMetadata);
            CallContext<GlobalContextPresentation>.SetData("GlobalContextPresentation", presentationContext);

            _phoenixGame = new PhoenixGame();
            _phoenixGameView = new PhoenixGameView(_phoenixGame);

            ContentManager content = new ContentManager(null);
            _phoenixGameView.LoadContent(content);
        }

        [Test]
        public void First()
        {
            _phoenixGameView.Update(0.0166666666666667f);
        }
    }
}