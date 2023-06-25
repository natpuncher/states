using System;
using UnityEngine;

namespace npg.states
{
	public abstract class StateMachine<TStateType> : IUpdatable, IFixedUpdatable, IDisposable
	{
		public Type ActiveStateType => _currentStateInfo?.StateType;
		public event Action<Type, Type> OnStateChanged;
		public virtual int StateHistorySize => 1;

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
			var lastStateType = ActiveStateType;
			var state = ChangeState<TState>();
			_currentStateInfo = _stateInfoPool.CreateStateInfo(state);
			state.Enter();
			NotifyStateChanged(lastStateType);
			return state;
		}

		public TState Enter<TState, TPayload>(TPayload payload) where TState : class, TStateType, IPayloadedState<TPayload>
		{
			var lastStateType = ActiveStateType;
			var state = ChangeState<TState>();
			_currentStateInfo = _stateInfoPool.CreateStateInfo(state, payload);
			state.Enter(payload);
			NotifyStateChanged(lastStateType);
			return state;
		}

		public bool Back()
		{
			if (_backHistory == null || !_backHistory.TryGetLastState(out var lastState))
			{
				return false;
			}

			_stateInfoPool.ReturnStateInfo(_currentStateInfo);
			lastState.ReEnter(this);
			return true;
		}

		public void ClearBackHistory()
		{
			if (_backHistory == null)
			{
				return;
			}
			
			foreach (var stateInfo in _backHistory.StateInfoHistory)
			{
				_stateInfoPool.ReturnStateInfo(stateInfo);
			}
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

		private TState ChangeState<TState>() where TState : class, IExitable
		{
			if (_currentStateInfo != null)
			{
				_currentStateInfo.Exit();
				AddToHistory(_currentStateInfo);
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
				_backHistory = new BackHistory<TStateType>(StateHistorySize);
			}
			
			_backHistory.Add(currentStateInfo);
		}

		protected virtual void StateChanged(Type oldStateType, Type newStateType)
		{
			Debug.Log($"[{GetType().Name}] {oldStateType?.Name} -> {newStateType?.Name}");
		}
	}
}