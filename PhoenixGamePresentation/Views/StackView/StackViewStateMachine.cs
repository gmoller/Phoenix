using Appccelerate.StateMachine;
using Appccelerate.StateMachine.Machine;

namespace PhoenixGamePresentation.Views.StackView
{
    internal class StackViewStateMachine
    {
        private enum States
        {
            Normal,
            Selected,
            ShowingPotentialMovement,
            Moving,
            Exploring,
            Patrolling,
            Fortified
        }

        private enum Events
        {
            Select,
            Unselect,
            ShowPotentialMovement,
            ResetPotentialMovement,
            Move,
            Explore,
            Patrol,
            Fortify
        }

        private PassiveStateMachine<States, Events> Machine { get; }

        internal StackViewStateMachine()
        {
            var builder = new StateMachineDefinitionBuilder<States, Events>();

            builder
                .In(States.Normal)
                .On(Events.Select)
                .Goto(States.Selected)
                .Execute<StackView>(TransitionToSelected);

            builder
                .In(States.Selected)
                .On(Events.Explore)
                .Goto(States.Exploring)
                .Execute<(StackView, WorldView)>(TransitionToExploring);

            builder
                .In(States.Selected)
                .On(Events.Move)
                .Goto(States.Moving)
                .Execute<StackView>(TransitionToMoving);

            builder
                .In(States.Selected)
                .On(Events.Unselect)
                .Goto(States.Normal)
                .Execute<StackView>(TransitionToNormal);

            builder
                .In(States.Selected)
                .On(Events.Patrol)
                .Goto(States.Patrolling)
                .Execute<StackView>(TransitionToPatrolling);

            builder
                .In(States.Selected)
                .On(Events.Fortify)
                .Goto(States.Fortified)
                .Execute<StackView>(TransitionToFortified);

            builder
                .In(States.Selected)
                .On(Events.ShowPotentialMovement)
                .Goto(States.ShowingPotentialMovement)
                .Execute<StackView>(TransitionToShowingPotentialMovement);

            builder
                .In(States.Moving)
                .On(Events.Select)
                .Goto(States.Selected)
                .Execute<StackView>(TransitionToSelected);

            builder
                .In(States.Moving)
                .On(Events.Unselect)
                .Goto(States.Normal)
                .Execute<StackView>(TransitionToNormal);

            builder
                .In(States.Exploring)
                .On(Events.Unselect)
                .Goto(States.Normal)
                .Execute<StackView>(TransitionToNormal);

            builder
                .In(States.Exploring)
                .On(Events.Move)
                .Goto(States.Moving)
                .Execute<StackView>(TransitionToMoving);

            builder
                .In(States.Patrolling)
                .On(Events.Select)
                .Goto(States.Selected)
                .Execute<StackView>(TransitionToSelected);

            builder
                .In(States.Fortified)
                .On(Events.Select)
                .Goto(States.Selected)
                .Execute<StackView>(TransitionToSelected);

            builder
                .In(States.ShowingPotentialMovement)
                .On(Events.ResetPotentialMovement)
                .Goto(States.Selected)
                .Execute<StackView>(TransitionToSelected);

            builder
                .WithInitialState(States.Normal);

            Machine = builder
                .Build()
                .CreatePassiveStateMachine();

            Machine.Start();
        }

        private void TransitionToNormal(StackView args)
        {
            args.SetStackViewState(new StackViewNormalState(args));
        }

        private void TransitionToSelected(StackView args)
        {
            args.SetStackViewState(new StackViewSelectedState(args));
        }

        private void TransitionToMoving(StackView args)
        {
            args.SetStackViewState(new StackViewMovingState(args));
        }

        private void TransitionToExploring((StackView stackView, WorldView worldView) args)
        {
            args.stackView.SetStackViewState(new StackViewExploringState(args.stackView, args.worldView));
        }

        private void TransitionToPatrolling(StackView args)
        {
            args.SetStackViewState(new StackViewPatrollingState(args));
        }

        private void TransitionToFortified(StackView args)
        {
            args.SetStackViewState(new StackViewFortifiedState(args));
        }

        private void TransitionToShowingPotentialMovement(StackView args)
        {
            args.SetStackViewState(new StackViewShowingPotentialMovementState(args));
        }

        internal void Select(StackView stackView)
        {
            Machine.Fire(Events.Select, stackView);
        }

        internal void Unselect(StackView stackView)
        {
            Machine.Fire(Events.Unselect, stackView);
        }

        internal void Move(StackView stackView)
        {
            Machine.Fire(Events.Move, stackView);
        }

        internal void Explore(StackView stackView, WorldView worldView)
        {
            Machine.Fire(Events.Explore, (stackView, worldView));
        }

        internal void Patrol(StackView stackView)
        {
            Machine.Fire(Events.Patrol, stackView);
        }

        internal void Fortify(StackView stackView)
        {
            Machine.Fire(Events.Fortify, stackView);
        }

        internal void ShowPotentialMovement(StackView stackView)
        {
            Machine.Fire(Events.ShowPotentialMovement, stackView);
        }

        internal void ResetPotentialMovement(StackView stackView)
        {
            Machine.Fire(Events.ResetPotentialMovement, stackView);
        }
    }
}