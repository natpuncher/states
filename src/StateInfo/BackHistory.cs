using System;
using npg.states.Infrastructure;
using UnityEngine;

namespace npg.states.StateInfo
{
	internal class BackHistory<TStateType> : IDisposable
	{
		private readonly IStateHandler<TStateType>[] _stateInfoHistory;
		private readonly StateHandlerPool<TStateType> _stateHandlerPool;
		private readonly int _capacity;

		private int _current;
		private int _historyCount;

		public BackHistory(StateHandlerPool<TStateType> stateHandlerPool, int capacity)
		{
			_stateHandlerPool = stateHandlerPool;
			_capacity = capacity;
			_stateInfoHistory = new IStateHandler<TStateType>[capacity];
		}

		public void Add(IStateHandler<TStateType> stateHandler)
		{
			if (_stateInfoHistory[_current] != null)
			{
				_stateHandlerPool.Return(_stateInfoHistory[_current]);
			}
			_stateInfoHistory[_current] = stateHandler;
			_current = (_current + 1) % _capacity;
			_historyCount = Mathf.Min(_historyCount + 1, _capacity);
		}

		public bool TryGetLastState(out IStateHandler<TStateType> state)
		{
			if (_historyCount <= 0)
			{
				state = default;
				return false;
			}

			_current = (_current + _capacity - 1) % _capacity;
			_historyCount--;
			state = _stateInfoHistory[_current];
			return true;
		}

		public void Dispose()
		{
			for (var i = 0; i < _stateInfoHistory.Length; i++)
			{
				_stateHandlerPool.Return(_stateInfoHistory[i]);
				_stateInfoHistory[i] = null;
			}
			_current = 0;
			_historyCount = 0;
		}
	}
}