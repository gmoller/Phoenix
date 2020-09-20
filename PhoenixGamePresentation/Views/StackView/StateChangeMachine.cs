//using System;
//using System.Collections.Generic;
//using Utilities;

//namespace PhoenixGamePresentation.Views.StackView
//{
//    internal readonly struct StateTransition
//    {
//        internal int From { get; }
//        internal int To { get; }
//        internal object State { get; }

//        internal StateTransition(int from, int to, object state)
//        {
//            From = from;
//            To = to;
//            State = state;
//        }
//    }

//    internal static class StateChangeMachine
//    {
//        private static readonly Dictionary<string, bool> StateTransitions = new Dictionary<string, bool>
//        {
//            { "0->1", true }, // normal->selected
//            { "1->0", true }, // selected->normal
//            { "1->2", true }, // selected->moving
//            { "1->3", true }, // selected->exploring
//            { "2->1", true }, // moving->selected
//            { "2->0", true }, // moving->normal
//            { "3->0", true }, // exploring->normal
//            { "3->1", true }, // exploring->selected
//            { "3->2", true }  // exploring->moving
//        };

//        internal static StackViewState Transition(StackView stackView, StateTransition transition)
//        {
//            if (stackView.StackViewState.Id != transition.From) throw new Exception($"Cannot transition from state [{transition.From}] because state is in [{stackView.StackViewState.Id}]");

//            var valid = StateTransitions.ContainsKey($"{transition.From}->{transition.To}");

//            if (!valid) throw new Exception($"Invalid transition from state [{transition.From}] to [{transition.To}]");

//            StackViewState newState = transition.To switch
//            {
//                0 => new StackViewNormalState(stackView),
//                1 => new StackViewSelectedState(stackView),
//                2 => new StackViewMovingState((PointI) transition.State, stackView),
//                3 => new StackViewExploringState(stackView),
//                _ => throw new Exception()
//            };

//            return newState;
//        }
//    }
//}