using System;

namespace npg.states.Infrastructure
{
	internal interface IStateHandler<TStateType> : IUpdatable, ILateUpdatable, IFixedUpdatable, IDisposable
	{
		Type StateType { get; }
		void ReEnter(StateMachine<TStateType> stateMachine, Type lastStateType);
		void Exit();
	}
}