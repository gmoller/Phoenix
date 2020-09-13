using System;
using System.Collections.Generic;

namespace PhoenixGamePresentation
{
    public class ActionArgs : EventArgs
    {
        public float DeltaTime { get; }

        public ActionArgs(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }

    internal class IfThenElseProcessor
    {
        #region State
        private readonly Dictionary<string, (Func<object, float, bool> ifFunc, Action<object, ActionArgs> thenAction, Action<object, ActionArgs> elseAction, object sender)> _conditionals;
        #endregion End State

        internal IfThenElseProcessor()
        {
            _conditionals = new Dictionary<string, (Func<object, float, bool> ifFunc, Action<object, ActionArgs> thenAction, Action<object, ActionArgs> elseAction, object sender)>();
        }

        internal void Add(string source, int id, object sender, Func<object, float, bool> ifFunc, Action<object, ActionArgs> thenAction, Action<object, ActionArgs> elseAction = null)
        {
            var key = $"{source}.{id}";
            _conditionals.Add(key, (ifFunc, thenAction, elseAction, sender));
        }

        internal void Remove(string key)
        {
            _conditionals.Remove(key);
        }

        internal void Update(float deltaTime)
        {
            foreach (var (ifFunc, thenAction, elseAction, sender) in _conditionals.Values)
            {
                if (ifFunc(sender, deltaTime))
                {
                    thenAction.Invoke(sender, new ActionArgs(deltaTime));
                }
                else
                {
                    elseAction?.Invoke(sender, new ActionArgs(deltaTime));
                }
            }
        }
    }
}