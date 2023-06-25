using System;

namespace npg.states
{
	internal interface IStateInfo<TStateType> : IUpdatable, IFixedUpdatable, IDisposable
	{
		Type StateType { get; }
		Type PayloadType { get; }
		void ReEnter(StateMachine<TStateType> stateMachine);
		void Exit();
	}
}