using System;
using System.Collections.Generic;
using npg.states.Infrastructure;

namespace npg.states.tests
{
	public class TestStateFactory : IStateFactory
	{
		private readonly Dictionary<Type, IExitable> _states = new Dictionary<Type, IExitable>
		{
			{typeof(TestState1), new TestState1()},
			{typeof(TestState2), new TestState2()},
			{typeof(TestPayloadState1), new TestPayloadState1()},
			{typeof(TestPayloadState2), new TestPayloadState2()},
		};

		public object GetState(Type stateType)
		{
			_states.TryGetValue(stateType, out var state);
			return state;
		}

		public TState GetState<TState>() where TState : class, IExitable
		{
			return GetState(typeof(TState)) as TState;
		}
	}
	
	public class TestState1 : ITestGameState, IState, IUpdatable, ILateUpdatable, IFixedUpdatable
	{
		public bool IsActive { get; private set; }
		public int UpdateCount { get; private set; }
		public int LateUpdateCount { get; private set; }
		public int FixedUpdateCount { get; private set; }
		
		public void Enter()
		{
			IsActive = true;
		}

		public void Exit()
		{
			IsActive = false;
			UpdateCount = 0;
			LateUpdateCount = 0;
			FixedUpdateCount = 0;
		}

		public void Update()
		{
			UpdateCount++;
		}

		public void LateUpdate()
		{
			LateUpdateCount++;
		}

		public void FixedUpdate()
		{
			FixedUpdateCount++;
		}
	}
	
	public class TestState2 : ITestGameState, IState, IUpdatable, ILateUpdatable, IFixedUpdatable
	{
		public bool IsActive { get; private set; }
		public int UpdateCount { get; private set; }
		public int LateUpdateCount { get; private set; }
		public int FixedUpdateCount { get; private set; }
		
		public void Enter()
		{
			IsActive = true;
		}

		public void Exit()
		{
			IsActive = false;
			UpdateCount = 0;
			LateUpdateCount = 0;
			FixedUpdateCount = 0;
		}

		public void Update()
		{
			UpdateCount++;
		}

		public void LateUpdate()
		{
			LateUpdateCount++;
		}

		public void FixedUpdate()
		{
			FixedUpdateCount++;
		}
	}

	public class TestPayloadState1 : ITestGameState, IPayloadedState<string>
	{
		public bool IsActive { get; private set; }
		public string Payload { get; private set; }
		
		public void Enter(string payload)
		{
			Payload = payload;
			IsActive = true;
		}

		public void Exit()
		{
			IsActive = false;
		}
	}
	
	public class TestPayloadState2 : ITestGameState, IPayloadedState<string>
	{
		public bool IsActive { get; private set; }
		public string Payload { get; private set; }
		
		public void Enter(string payload)
		{
			Payload = payload;
			IsActive = true;
		}

		public void Exit()
		{
			IsActive = false;
		}
	}
}