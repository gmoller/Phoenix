﻿using Microsoft.Xna.Framework.Graphics;
using Zen.Input;
using Zen.MonoGameUtilities.ViewportAdapters;
using Zen.Utilities;

namespace PhoenixGamePresentation.Views
{
    internal abstract class ViewBase
    {
        #region State
        internal long Id { get; set; }

        protected WorldView WorldView { get; set; }
        protected InputHandler Input { get; set; }

        protected Viewport Viewport { get; private set; }
        protected ViewportAdapter ViewportAdapter { get; set; }

        protected bool IsDisposed { get; set; }
        #endregion

        protected void SetupViewport(int x, int y, int width, int height)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            Viewport = new Viewport(x, y, width, height, 0.0f, 1.0f);
            ViewportAdapter = new ScalingViewportAdapter(context.GraphicsDevice, width, height);
        }
    }
}