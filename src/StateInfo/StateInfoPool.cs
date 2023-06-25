using System;
using System.Collections.Generic;
using npg.states.Infrastructure;

namespace npg.states.StateInfo
{
	internal class StateInfoPool<TStateType> : IDisposable
	{
		private Stack<IStateInfo<TStateType>> _stateInfos = new Stack<IStateInfo<TStateType>>();
		private Dictionary<Type, Stack<IStateInfo<TStateType>>> _payloadedStateInfos = new Dictionary<Type, Stack<IStateInfo<TStateType>>>();

		public IStateInfo<TStateType> CreateStateInfo<TState>(TState state) where TState : class, TStateType, IState
		{
			if (_stateInfos.TryPop(out var result))
			{
				((StateInfo<TState, TStateType>)result).Initialize(state);
			}
			else
			{
				var stateInfo = new StateInfo<TState, TStateType>();
				stateInfo.Initialize(state);
				result = stateInfo;
			}
			return result;
		}
		
		public IStateInfo<TStateType> CreateStateInfo<TPayloadedState, TPayload>(TPayloadedState state, TPayload payload) 
			where TPayloadedState : class, TStateType, IPayloadedState<TPayload>
		{
			// var payloadType = typeof(TPayload);
			// if (!_payloadedStateInfos.TryGetValue(payloadType, out var pool))
			// {
			// 	pool = new Stack<IStateInfo<TStateType>>();
			// 	_payloadedStateInfos[payloadType] = pool;
			// }
			//
			// if (pool.TryPop(out var result))
			// {
			// 	((PayloadedStateInfo<TPayloadedState, TStateType, TPayload>)result).Initialize(state, payload);
			// }
			// else
			// {
				var stateInfo = new PayloadedStateInfo<TPayloadedState, TStateType, TPayload>();
				stateInfo.Initialize(state, payload);
				return stateInfo;
				// result = stateInfo;
				// }
				// return result;
		}

		public void ReturnStateInfo(IStateInfo<TStateType> stateInfo)
		{
			if (stateInfo == null)
			{
				return;
			}
			
			if (stateInfo.PayloadType != null)
			{
				ReturnPayloadedStateInfo(stateInfo);
				return;
			}
			
			stateInfo.Dispose();
			_stateInfos.Push(stateInfo);
		}

		private void ReturnPayloadedStateInfo(IStateInfo<TStateType> stateInfo)
		{
			stateInfo.Dispose();
			if (!_payloadedStateInfos.TryGetValue(stateInfo.PayloadType, out var pool))
			{
				pool = new Stack<IStateInfo<TStateType>>();
				_payloadedStateInfos[stateInfo.PayloadType] = pool;
			}
			
			pool.Push(stateInfo);
		}

		public void Dispose()
		{
			_stateInfos.Clear();
			_payloadedStateInfos.Clear();
		}
	}
}