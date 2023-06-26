using System;
using System.Collections.Generic;
using npg.states.Infrastructure;

namespace npg.states.StateInfo
{
	internal class StateHandlerPool<TStateType> : IDisposable
	{
		private readonly Dictionary<Type, Stack<IStateHandler<TStateType>>> _stateHandlers = 
			new Dictionary<Type, Stack<IStateHandler<TStateType>>>();

		public IStateHandler<TStateType> GetStateHandler<TState>(TState state) where TState : class, TStateType, IState
		{
			var stateType = typeof(TState);
			if (!_stateHandlers.TryGetValue(stateType, out var pool))
			{
				pool = new Stack<IStateHandler<TStateType>>();
				_stateHandlers[stateType] = pool;
			}
			
			if (pool.TryPop(out var result))
			{
				((StateHandler<TState, TStateType>)result).Initialize(state);
			}
			else
			{
				var stateInfo = new StateHandler<TState, TStateType>();
				stateInfo.Initialize(state);
				result = stateInfo;
			}

			return result;
		}
		
		public IStateHandler<TStateType> GetStateHandler<TPayloadedState, TPayload>(TPayloadedState state, TPayload payload) 
			where TPayloadedState : class, TStateType, IPayloadedState<TPayload>
		{
			var stateType = typeof(TPayloadedState);
			if (!_stateHandlers.TryGetValue(stateType, out var pool))
			{
				pool = new Stack<IStateHandler<TStateType>>();
				_stateHandlers[stateType] = pool;
			}

			if (pool.TryPop(out var result))
			{
				((PayloadedStateHandler<TPayloadedState, TStateType, TPayload>)result).Initialize(state, payload);
			}
			else
			{
				var stateInfo = new PayloadedStateHandler<TPayloadedState, TStateType, TPayload>();
				stateInfo.Initialize(state, payload);
				result = stateInfo;
			}

			return result;
		}

		public void Return(IStateHandler<TStateType> stateHandler)
		{
			if (stateHandler == null)
			{
				return;
			}
			
			if (!_stateHandlers.TryGetValue(stateHandler.StateType, out var pool))
			{
				pool = new Stack<IStateHandler<TStateType>>();
				_stateHandlers[stateHandler.StateType] = pool;
			}
			
			stateHandler.Dispose();
			pool.Push(stateHandler);
		}

		public void Dispose()
		{
			_stateHandlers.Clear();
		}
	}
}