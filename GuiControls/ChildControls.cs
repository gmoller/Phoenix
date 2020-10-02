﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Utilities;

namespace GuiControls
{
    public class ChildControls : IEnumerable<IControl>
    {
        private Dictionary<string, IControl> ChildControlsList { get; }

        internal ChildControls()
        {
            ChildControlsList = new Dictionary<string, IControl>();
        }

        public IControl this[int index] => ChildControlsList.Values.ElementAt(index);
        private int Count => ChildControlsList.Count;

        internal void Add(string key, IControl control)
        {
            ChildControlsList.Add(key, control);
        }

        internal IControl FindControl(string key)
        {
            var split = key.Split('.');
            var childControl = ChildControlsList[split[0]];
            for (var i = 1; i < split.Length; i++)
            {
                childControl = childControl[split[i]];
            }

            return childControl;
        }

        internal void LoadChildControls(ContentManager content, bool loadChildrenContent)
        {
            foreach (var childControl in ChildControlsList.Values)
            {
                childControl.LoadContent(content, loadChildrenContent);
            }
        }

        internal void SetTopLeftPosition(PointI point)
        {
            foreach (var child in ChildControlsList.Values)
            {
                child.SetTopLeftPosition(point + child.RelativeTopLeft);
            }
        }

        internal void MoveTopLeftPosition(PointI point)
        {
            foreach (var child in ChildControlsList.Values)
            {
                child.MoveTopLeftPosition(point);
            }
        }

        internal void UpdateChildControls(InputHandler input, float deltaTime, Viewport? viewport)
        {
            foreach (var childControl in ChildControlsList.Values)
            {
                childControl.Update(input, deltaTime, viewport);
            }
        }

        internal void DrawChildControls(SpriteBatch spriteBatch)
        {
            foreach (var childControl in ChildControlsList.Values)
            {
                childControl.Draw(spriteBatch);
            }
        }

        internal void DrawChildControls(Matrix? transform = null)
        {
            foreach (var childControl in ChildControlsList.Values)
            {
                childControl.Draw(transform);
            }
        }

        public IEnumerator<IControl> GetEnumerator()
        {
            foreach (var item in ChildControlsList)
            {
                yield return item.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Count={Count}}}";
    }
}