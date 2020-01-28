using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class Tests
	{
		[Test]
		public void Test()
		{
			var stateMachine = new ExampleStateMachine(new ExampleStateFactory());
			stateMachine.Update();
			stateMachine.Enter<ExampleState>();
			stateMachine.Update();
			
			Assert.IsTrue(stateMachine.ActiveStateType == typeof(ExampleState));

			var payloadedState = stateMachine.Enter<ExamplePayloadedState, int>(15);
			
			stateMachine.Update();
			
			Assert.AreEqual(15, payloadedState.Payload);
			Assert.IsTrue(stateMachine.ActiveStateType == typeof(ExamplePayloadedState));

			Assert.IsTrue(stateMachine.Back());
			
			Assert.IsTrue(stateMachine.ActiveStateType == typeof(ExampleState));
			
			Assert.IsTrue(stateMachine.Back());
			
			Assert.IsTrue(stateMachine.ActiveStateType == typeof(ExamplePayloadedState));
		}
	}
}