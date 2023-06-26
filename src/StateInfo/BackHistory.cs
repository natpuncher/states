using System;
using npg.states.Infrastructure;
using UnityEngine;

namespace npg.states.StateInfo
{
	internal class BackHistory<TStateType> : IDisposable
	{
		private readonly IStateInfo<TStateType>[] _stateInfoHistory;
		private readonly StateInfoPool<TStateType> _stateInfoPool;
		private readonly int _capacity;

		private int _current;
		private int _historyCount;

		public BackHistory(StateInfoPool<TStateType> stateInfoPool, int capacity)
		{
			_stateInfoPool = stateInfoPool;
			_capacity = capacity;
			_stateInfoHistory = new IStateInfo<TStateType>[capacity];
		}

		public void Add(IStateInfo<TStateType> stateInfo)
		{
			if (_stateInfoHistory[_current] != null)
			{
				_stateInfoPool.ReturnStateInfo(_stateInfoHistory[_current]);
			}
			_stateInfoHistory[_current] = stateInfo;
			_current = (_current + 1) % _capacity;
			_historyCount = Mathf.Min(_historyCount + 1, _capacity);
		}

		public bool TryGetLastState(out IStateInfo<TStateType> state)
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
				_stateInfoPool.ReturnStateInfo(_stateInfoHistory[i]);
				_stateInfoHistory[i] = null;
			}
			_current = 0;
			_historyCount = 0;
		}
	}
}