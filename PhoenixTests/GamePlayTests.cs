using Microsoft.Xna.Framework.Content;
using NUnit.Framework;
using PhoenixGameData;
using PhoenixGamePresentation;
using Zen.Utilities;

namespace PhoenixTests
{
    [TestFixture]
    public class GamePlayTests
    {
        private PhoenixGameView _phoenixGameView;

        [SetUp]
        public void Setup()
        {
            // Arrange
            var gameMetadata = new GameConfigCache();
            var presentationContext = new GlobalContextPresentation();
            CallContext<GameConfigCache>.SetData("GameMetadata", gameMetadata);
            CallContext<GlobalContextPresentation>.SetData("GlobalContextPresentation", presentationContext);

            _phoenixGameView = new PhoenixGameView();

            ContentManager content = new ContentManager(null);
            _phoenixGameView.LoadContent(content);
        }

        [Test]
        public void First()
        {
            
        }
    }
}