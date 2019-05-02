using NPG.States;

namespace Tests
{
	public class ExampleStateMachine : StateMachine
	{
		protected override IStateFactory Factory
		{
			get
			{
				if (_stateFactory == null)
				{
					_stateFactory = new ExampleStateFactory();
				}

				return _stateFactory;
			}
		}

		private IStateFactory _stateFactory;
	}
}