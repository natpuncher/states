using NUnit.Framework;

namespace npg.states.tests
{
	public class BackHistoryTests
	{
		[Test]
		public void BackTest()
		{
			var stateMachine = new TestGameStateMachine(new TestStateFactory());

			stateMachine.Enter<TestState1>();
			Assert.AreEqual(typeof(TestState1), stateMachine.ActiveStateType);
			
			stateMachine.Enter<TestState2>();
			Assert.AreEqual(typeof(TestState2), stateMachine.ActiveStateType);

			var backSuccessful = stateMachine.Back();
			Assert.IsTrue(backSuccessful);
			Assert.AreEqual(typeof(TestState1), stateMachine.ActiveStateType);
		}

		[Test]
		public void OverflowBackTest()
		{
			var stateMachine = new TestGameStateMachine(new TestStateFactory());
			
			stateMachine.Enter<TestState1>();
			stateMachine.Enter<TestState2>();
			stateMachine.Enter<TestPayloadState1, string>("1");
			
			var backSuccessful = stateMachine.Back();
			Assert.IsTrue(backSuccessful);
			Assert.AreEqual(typeof(TestState2), stateMachine.ActiveStateType);
			
			backSuccessful = stateMachine.Back();
			Assert.IsFalse(backSuccessful);
			Assert.AreEqual(typeof(TestState2), stateMachine.ActiveStateType);
		}

		[Test]
		public void ResetBackTest()
		{
			var stateMachine = new TestGameStateMachine(new TestStateFactory());
			
			stateMachine.Enter<TestState1>();
			stateMachine.Enter<TestState2>();
			stateMachine.ClearBackHistory();
			
			var backSuccessful = stateMachine.Back();
			Assert.IsFalse(backSuccessful);
			Assert.AreEqual(typeof(TestState2), stateMachine.ActiveStateType);
		}

		[Test]
		public void BackPreservePayloadTest()
		{
			var stateMachine = new TestGameStateMachine(new TestStateFactory());
			
			stateMachine.Enter<TestPayloadState1, string>("1");
			var state2 = stateMachine.Enter<TestPayloadState1, string>("2");
			stateMachine.Enter<TestPayloadState1, string>("3");
			
			stateMachine.Back();
			Assert.IsTrue(state2.IsActive);
			Assert.AreEqual("2", state2.Payload);
		}
	}
}