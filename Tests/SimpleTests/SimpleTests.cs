using NUnit.Framework;

namespace NPG.States.Test
{
	[TestFixture]
	public class SimpleTests
	{
		[Test]
		public void SimpleTest()
		{
			var sm = new StateMachine();
			
			Assert.Null(sm.CurrentState);
			
			var simpleState = new SimpleState(sm);
			var payloadState = new SimplePayloadState(sm);
			
			simpleState.Enter();
			
			Assert.AreEqual(sm.CurrentState, simpleState);

			var name = "Nagibator";
			var level = 999;
			payloadState.Enter(new SimplePayload(name, level));
			
			Assert.AreEqual(payloadState.SelectedCharacterName, name);
			Assert.AreEqual(payloadState.SelectedCharacterLevel, level);
			Assert.AreEqual(sm.CurrentState, payloadState);
			
			simpleState.Enter();
		}

		[Test]
		public void ChainTest()
		{
			var sm = new StateMachine();

			var lastState = new ChainState(sm);
			var state = new ChainState(sm, new ChainState(sm, lastState));
			
			var name = "Nagibator";
			var level = 999;
			state.Enter(new SimplePayload(name, level));
			
			Assert.AreEqual(sm.CurrentState, lastState);
		}
	}
}