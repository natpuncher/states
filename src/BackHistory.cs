using System;
using UnityEngine;

namespace npg.states
{
	internal class BackHistory<TStateType> : IDisposable
	{
		public IStateInfo<TStateType>[] StateInfoHistory => _stateInfoHistory;
		
		private readonly int _capacity;
		private readonly IStateInfo<TStateType>[] _stateInfoHistory;


		private int _current;
		private int _historyCount;

		public BackHistory(int capacity)
		{
			_capacity = capacity;
			_stateInfoHistory = new IStateInfo<TStateType>[capacity];
		}

		public void Add(IStateInfo<TStateType> stateInfo)
		{
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
			state = _stateInfoHistory[_current];
			_historyCount--;
			return true;
		}

		public void Dispose()
		{
			for (var i = 0; i < _stateInfoHistory.Length; i++)
			{
				_stateInfoHistory[i] = null;
			}
			_current = 0;
			_historyCount = 0;
		}
	}
}