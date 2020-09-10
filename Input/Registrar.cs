using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Input
{
    public class Registrar
    {
        private bool RegistrationBegun { get; set; }
        private string GameStatus { get; set; }
        private string Owner { get; set; }
        private InputHandler Input { get; }

        private Dictionary<string, (int id, object sender, Keys keys, KeyboardInputActionType inputActionType, Action<object, KeyboardEventArgs> action)> KeyboardEventHandlersRepository { get; }
        private Dictionary<string, (int id, object sender, MouseInputActionType inputActionType, Action<object, MouseEventArgs> action)> MouseEventHandlersRepository { get; }

        public Registrar(InputHandler input)
        {
            KeyboardEventHandlersRepository = new Dictionary<string, (int id, object sender, Keys keys, KeyboardInputActionType inputActionType, Action<object, KeyboardEventArgs> action)>();
            MouseEventHandlersRepository = new Dictionary<string, (int id, object sender, MouseInputActionType inputActionType, Action<object, MouseEventArgs> action)>();

            Input = input;
        }

        public void BeginRegistration(string gameStatus, string owner)
        {
            if (RegistrationBegun) throw new Exception("Registration already begun.");

            RegistrationBegun = true;
            GameStatus = gameStatus;
            Owner = owner;
        }

        public void Register(int id, object sender, Keys keys, KeyboardInputActionType inputActionType, Action<object, KeyboardEventArgs> action)
        {
            var key = $"{GameStatus}.{Owner}.{id}";
            KeyboardEventHandlersRepository.Add(key, (id, sender, keys, inputActionType, action));
        }

        public void Register(int id, object sender, MouseInputActionType inputActionType, Action<object, MouseEventArgs> action)
        {
            var key = $"{GameStatus}.{Owner}.{id}";
            MouseEventHandlersRepository.Add(key, (id, sender, inputActionType, action));
        }

        public void EndRegistration()
        {
            if (!RegistrationBegun) throw new Exception("Registration has not begun.");

            RegistrationBegun = false;
            GameStatus = string.Empty;
            Owner = string.Empty;
        }

        public void Subscribe(string gameStatus, string owner)
        {
            foreach (var item in KeyboardEventHandlersRepository)
            {
                if (item.Key.StartsWith($"{gameStatus}.{owner}."))
                {
                    Input.SubscribeToEventHandler(owner, item.Value.id, item.Value.sender, item.Value.keys, item.Value.inputActionType, item.Value.action);
                }
            }

            foreach (var item in MouseEventHandlersRepository)
            {
                if (item.Key.StartsWith($"{gameStatus}.{owner}."))
                {
                    Input.SubscribeToEventHandler(owner, item.Value.id, item.Value.sender, item.Value.inputActionType, item.Value.action);
                }
            }
        }

        public void Unsubscribe(string gameStatus, string owner)
        {
            foreach (var item in KeyboardEventHandlersRepository)
            {
                if (item.Key.StartsWith($"{gameStatus}.{owner}."))
                {
                    Input.UnsubscribeFromEventHandler(owner, item.Value.id, item.Value.inputActionType);
                }
            }

            foreach (var item in MouseEventHandlersRepository)
            {
                if (item.Key.StartsWith($"{gameStatus}.{owner}."))
                {
                    Input.UnsubscribeFromEventHandler(owner, item.Value.id, item.Value.inputActionType);
                }
            }
        }
    }
}