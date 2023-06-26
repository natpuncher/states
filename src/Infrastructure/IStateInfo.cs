using System;

namespace npg.states.Infrastructure
{
	internal interface IStateInfo<TStateType> : IUpdatable, IFixedUpdatable, IDisposable
	{
		Type StateType { get; }
		Type PayloadType { get; }
		void ReEnter(StateMachine<TStateType> stateMachine, Type lastStateType);
		void Exit();
	}
}