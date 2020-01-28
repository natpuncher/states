using System;
using System.Collections.Generic;
using NPG.States;

namespace Tests
{
	public class ExampleStateFactory : IStateFactory
	{
		private readonly Dictionary<Type, IExitable> _states = new Dictionary<Type, IExitable>
		{
			{typeof(ExampleState), new ExampleState()},
			{typeof(ExamplePayloadedState), new ExamplePayloadedState()}
		};
		
		public T GetState<T>() where T : class, IExitable
		{
			var type = typeof(T);
			IExitable state;
			if (!_states.TryGetValue(type, out state))
			{
				throw new Exception("State can't be created");
			}
			return state as T;
		}
	}
}