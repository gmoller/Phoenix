using System;
using System.Collections.Generic;
using Input;

namespace PhoenixGamePresentation.Handlers
{
    internal class GameStatusHandler
    {
        #region State
        private Dictionary<string, (GameStatus from, GameStatus to)> AllowedStatusChanges { get; }

        private Dictionary<string, Action<GameStatus, GameStatus, string>> Subscriptions { get; }
        internal GameStatus GameStatus { get; private set; }
        #endregion

        internal GameStatusHandler(GameStatus gameStatus)
        {
            GameStatus = gameStatus;

            AllowedStatusChanges = new Dictionary<string, (GameStatus from, GameStatus to)>
            {
                {"OverlandMap->CityView", (GameStatus.OverlandMap, GameStatus.CityView)},
                {"CityView->OverlandMap", (GameStatus.CityView, GameStatus.OverlandMap)}
            };
            Subscriptions = new Dictionary<string, Action<GameStatus, GameStatus, string>>();
        }

        internal void ChangeStatus(GameStatus from, GameStatus to)
        {
            if (GameStatus != from) throw new Exception($"Expected GameStatus [{from}], was [{GameStatus}]");

            var key = $"{from}->{to}";
            if (AllowedStatusChanges.ContainsKey(key))
            {
                var foo = AllowedStatusChanges[key];
                GameStatus = to;
                OnGameStatusChange(foo.from, foo.to);
            }
            else
            {
                throw new Exception($"GameStatus can not be changed from [{from}] to [{to}]");
            }
        }

        private void OnGameStatusChange(GameStatus from, GameStatus to)
        {
            foreach (var subscription in Subscriptions)
            {

                subscription.Value.Invoke(from, to, subscription.Key);
            }
        }

        internal void SubscribeToStatusChanges(string owner, Action<GameStatus, GameStatus, string> action)
        {
            var key = BuildKey(owner);
            Subscriptions.Add(key, action);
        }

        internal void UnsubscribeFromStatusChanges(string owner)
        {
            var key = BuildKey(owner);
            Subscriptions.Remove(key);
        }

        private string BuildKey(string owner)
        {
            var key = $"{owner}";

            return key;
        }
    }
}