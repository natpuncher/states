using System;
using npg.states.Infrastructure;
using npg.states.StateInfo;
using UnityEngine;

namespace npg.states
{
	public abstract class StateMachine<TStateType> : IUpdatable, IFixedUpdatable, IDisposable
	{
		public virtual int BackHistorySize => 1;
		
		public Type ActiveStateType => _currentStateInfo?.StateType;
		public event Action<Type, Type> OnStateChanged;

		private readonly IStateFactory _stateFactory;
		private readonly StateInfoPool<TStateType> _stateInfoPool = new StateInfoPool<TStateType>();
		private BackHistory<TStateType> _backHistory;

		private IStateInfo<TStateType> _currentStateInfo;

		protected StateMachine(IStateFactory stateFactory)
		{
			_stateFactory = stateFactory;
		}

		public TState Enter<TState>() where TState : class, TStateType, IState
		{
			return InternalEnter<TState>(ActiveStateType);
		}

		public TState Enter<TState, TPayload>(TPayload payload) where TState : class, TStateType, IPayloadedState<TPayload>
		{
			return InternalEnter<TState, TPayload>(ActiveStateType, payload);
		}

		public bool Back()
		{
			if (_backHistory == null || !_backHistory.TryGetLastState(out var previousStateInfo))
			{
				return false;
			}

			var lastStateType = ActiveStateType;
			_stateInfoPool.ReturnStateInfo(_currentStateInfo);
			previousStateInfo.ReEnter(this, lastStateType);
			return true;
		}

		public void ClearBackHistory()
		{
			_backHistory?.Dispose();
		}

		public void Update()
		{
			_currentStateInfo?.Update();
		}

		public void FixedUpdate()
		{
			_currentStateInfo?.FixedUpdate();
		}

		public void Dispose()
		{
			_currentStateInfo?.Exit();
			_currentStateInfo = null;

			_backHistory?.Dispose();
		}

		internal TState InternalEnter<TState>(Type lastStateType, bool addToHistory = true) where TState : class, TStateType, IState
		{
			var state = ChangeState<TState>(addToHistory);
			_currentStateInfo = _stateInfoPool.CreateStateInfo(state);
			NotifyStateChanged(lastStateType);
			state.Enter();
			return state;
		}
		
		internal TState InternalEnter<TState, TPayload>(Type lastStateType, TPayload payload, bool addToHistory = true)
			where TState : class, TStateType, IPayloadedState<TPayload>
		{
			var state = ChangeState<TState>(addToHistory);
			_currentStateInfo = _stateInfoPool.CreateStateInfo(state, payload);
			NotifyStateChanged(lastStateType);
			state.Enter(payload);
			return state;
		}

		private TState ChangeState<TState>(bool addToHistory = true) where TState : class, IExitable
		{
			if (_currentStateInfo != null)
			{
				_currentStateInfo.Exit();
				if (addToHistory)
				{
					AddToHistory(_currentStateInfo);	
				}
			}

			return _stateFactory.GetState<TState>();
		}
		
		private void NotifyStateChanged(Type lastStateType)
		{
			StateChanged(lastStateType, ActiveStateType);
			OnStateChanged?.Invoke(lastStateType, ActiveStateType);
		}

		private void AddToHistory(IStateInfo<TStateType> currentStateInfo)
		{
			if (_backHistory == null)
			{
				_backHistory = new BackHistory<TStateType>(_stateInfoPool, BackHistorySize);
			}
			
			_backHistory.Add(currentStateInfo);
		}

		protected virtual void StateChanged(Type oldStateType, Type newStateType)
		{
			Debug.Log($"[{GetType().Name}] {oldStateType?.Name} -> {newStateType?.Name}");
		}
	}
}