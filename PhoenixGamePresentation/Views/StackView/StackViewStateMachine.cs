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
            Moving,
            Exploring,
            Patrolling,
            Fortified
        }

        private enum Events
        {
            Select,
            Unselect,
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
                .Execute<StackView>(TransitionToExploring);

            builder
                .In(States.Selected)
                .On(Events.Move)
                .Goto(States.Moving)
                .Execute((StackView stackView) => { TransitionToMoving(stackView); });

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
                .WithInitialState(States.Normal);

            Machine = builder
                .Build()
                .CreatePassiveStateMachine();

            Machine.Start();
        }

        private void TransitionToNormal(StackView stackView)
        {
            stackView.SetStackViewState(new StackViewNormalState(stackView));
        }

        private void TransitionToSelected(StackView stackView)
        {
            stackView.SetStackViewState(new StackViewSelectedState(stackView));
        }

        private void TransitionToMoving(StackView stackView)
        {
            stackView.SetStackViewState(new StackViewMovingState(stackView));
        }

        private void TransitionToExploring(StackView stackView)
        {
            stackView.SetStackViewState(new StackViewExploringState(stackView));
        }

        private void TransitionToPatrolling(StackView stackView)
        {
            stackView.SetStackViewState(new StackViewPatrollingState(stackView));
        }

        private void TransitionToFortified(StackView stackView)
        {
            stackView.SetStackViewState(new StackViewFortifiedState(stackView));
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

        internal void Explore(StackView stackView)
        {
            Machine.Fire(Events.Explore, stackView);
        }

        internal void Patrol(StackView stackView)
        {
            Machine.Fire(Events.Patrol, stackView);
        }

        internal void Fortify(StackView stackView)
        {
            Machine.Fire(Events.Fortify, stackView);
        }
    }
}