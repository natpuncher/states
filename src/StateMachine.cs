using System;
using npg.states.Infrastructure;
using npg.states.StateInfo;
using UnityEngine;

namespace npg.states
{
	public abstract class StateMachine<TStateType> : IUpdatable, ILateUpdatable, IFixedUpdatable, IDisposable
	{
		public virtual int BackHistorySize => 1;
		
		public Type ActiveStateType => _currentStateHandler?.StateType;
		public event Action<Type, Type> OnStateChanged;

		private readonly IStateFactory _stateFactory;
		private readonly StateHandlerPool<TStateType> _stateHandlerPool = new StateHandlerPool<TStateType>();
		private BackHistory<TStateType> _backHistory;

		private IStateHandler<TStateType> _currentStateHandler;

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
			_stateHandlerPool.Return(_currentStateHandler);
			previousStateInfo.ReEnter(this, lastStateType);
			return true;
		}

		public void ClearBackHistory()
		{
			_backHistory?.Dispose();
		}

		public void Update()
		{
			_currentStateHandler?.Update();
		}

		public void LateUpdate()
		{
			_currentStateHandler?.LateUpdate();
		}

		public void FixedUpdate()
		{
			_currentStateHandler?.FixedUpdate();
		}

		public void Dispose()
		{
			_currentStateHandler?.Exit();
			_currentStateHandler = null;

			_backHistory?.Dispose();
		}

		internal TState InternalEnter<TState>(Type lastStateType, bool addToHistory = true) where TState : class, TStateType, IState
		{
			var state = ChangeState<TState>(addToHistory);
			_currentStateHandler = _stateHandlerPool.GetStateHandler(state);
			NotifyStateChanged(lastStateType);
			state.Enter();
			return state;
		}
		
		internal TState InternalEnter<TState, TPayload>(Type lastStateType, TPayload payload, bool addToHistory = true)
			where TState : class, TStateType, IPayloadedState<TPayload>
		{
			var state = ChangeState<TState>(addToHistory);
			_currentStateHandler = _stateHandlerPool.GetStateHandler(state, payload);
			NotifyStateChanged(lastStateType);
			state.Enter(payload);
			return state;
		}

		private TState ChangeState<TState>(bool addToHistory = true) where TState : class, IExitable
		{
			if (_currentStateHandler != null)
			{
				_currentStateHandler.Exit();
				if (addToHistory)
				{
					AddToHistory(_currentStateHandler);	
				}
			}

			return _stateFactory.GetState<TState>();
		}
		
		private void NotifyStateChanged(Type lastStateType)
		{
			StateChanged(lastStateType, ActiveStateType);
			OnStateChanged?.Invoke(lastStateType, ActiveStateType);
		}

		private void AddToHistory(IStateHandler<TStateType> currentStateHandler)
		{
			if (_backHistory == null)
			{
				_backHistory = new BackHistory<TStateType>(_stateHandlerPool, BackHistorySize);
			}
			
			_backHistory.Add(currentStateHandler);
		}

		protected virtual void StateChanged(Type oldStateType, Type newStateType)
		{
			Debug.Log($"[{GetType().Name}] {oldStateType?.Name} -> {newStateType?.Name}");
		}
	}
}