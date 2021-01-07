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
        private PhoenixGameView _phoenixGameView;

        [SetUp]
        public void Setup()
        {
            // Arrange
            var gameMetadata = new GameMetadata();
            var presentationContext = new GlobalContextPresentation();
            CallContext<GameMetadata>.SetData("GameMetadata", gameMetadata);
            CallContext<GlobalContextPresentation>.SetData("GlobalContextPresentation", presentationContext);

            _phoenixGameView = new PhoenixGameView();

            ContentManager content = new ContentManager(null);
            _phoenixGameView.LoadContent(content);
        }

        [Test]
        public void First()
        {
            //_phoenixGameView.Update(0.0166666666666667f);
        }
    }
}