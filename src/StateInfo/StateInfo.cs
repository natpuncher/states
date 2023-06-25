using npg.states.Infrastructure;

namespace npg.states.StateInfo
{
	internal class StateInfo<TState, TStateType> : BaseStateInfo<TStateType> where TState : class, TStateType, IState
	{
		public void Initialize(TState state)
		{
			InternalInitialize(state);
		}

		public override void ReEnter(StateMachine<TStateType> stateMachine)
		{
			stateMachine.InternalEnter<TState>(false);
		}
	}
}