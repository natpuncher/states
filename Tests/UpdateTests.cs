using NUnit.Framework;

namespace npg.states.tests
{
	public class UpdateTests
	{
		[Test]
		public void UpdateTest()
		{
			var stateMachine = new TestGameStateMachine(new TestStateFactory());

			var state1 = stateMachine.Enter<TestState1>();
			var state2 = stateMachine.Enter<TestState2>();
			
			Assert.AreEqual(0, state1.UpdateCount);
			Assert.AreEqual(0, state1.LateUpdateCount);
			Assert.AreEqual(0, state1.FixedUpdateCount);
			
			Assert.AreEqual(0, state2.UpdateCount);
			Assert.AreEqual(0, state2.LateUpdateCount);
			Assert.AreEqual(0, state2.FixedUpdateCount);
			
			stateMachine.Update();
			
			stateMachine.LateUpdate();
			stateMachine.LateUpdate();
			
			stateMachine.FixedUpdate();
			stateMachine.FixedUpdate();
			stateMachine.FixedUpdate();
			
			Assert.AreEqual(0, state1.UpdateCount);
			Assert.AreEqual(0, state1.LateUpdateCount);
			Assert.AreEqual(0, state1.FixedUpdateCount);
			
			Assert.AreEqual(1, state2.UpdateCount);
			Assert.AreEqual(2, state2.LateUpdateCount);
			Assert.AreEqual(3, state2.FixedUpdateCount);
		}
	}
}