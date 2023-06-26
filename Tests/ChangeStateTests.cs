using NUnit.Framework;

namespace npg.states.tests
{
	public class ChangeStateTests
	{
		[Test]
		public void EnterExitTest()
		{
			var stateMachine = new TestGameStateMachine(new TestStateFactory());
			Assert.IsNull(stateMachine.ActiveStateType);

			var state1 = stateMachine.Enter<TestState1>();
			Assert.AreEqual(typeof(TestState1), stateMachine.ActiveStateType);
			Assert.IsTrue(state1.IsActive);
			
			var state2 = stateMachine.Enter<TestState2>();
			Assert.AreEqual(typeof(TestState2), stateMachine.ActiveStateType);
			Assert.IsFalse(state1.IsActive);
			Assert.IsTrue(state2.IsActive);
			
			stateMachine.Dispose();
			Assert.IsNull(stateMachine.ActiveStateType);
			Assert.IsFalse(state1.IsActive);
			Assert.IsFalse(state2.IsActive);
		}

		[Test]
		public void PayloadStateTest()
		{
			var stateMachine = new TestGameStateMachine(new TestStateFactory());

			var payload = "first";
			var state = stateMachine.Enter<TestPayloadState1, string>(payload);
			
			Assert.AreEqual(payload, state.Payload);

			var payload2 = "second";
			var state2 = stateMachine.Enter<TestPayloadState2, string>(payload2);

			Assert.AreEqual(payload, state.Payload);
			Assert.AreEqual(payload2, state2.Payload);

			Assert.AreEqual(typeof(TestPayloadState2), stateMachine.ActiveStateType);
			Assert.IsFalse(state.IsActive);
			Assert.IsTrue(state2.IsActive);
		}
	}
}