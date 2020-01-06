using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class Tests
	{
		[Test]
		public void Test()
		{
			var stateMachine = new GameStateMachine(new ExampleStateFactory());
			stateMachine.Update();
			var exampleState = stateMachine.Enter<ExampleState>();
			stateMachine.Update();
			
			Assert.IsTrue(stateMachine.IsActive(exampleState));
			Assert.IsTrue(stateMachine.IsActive(typeof(ExampleState)));

			// stateMachine.Enter<WrongPayloadedState, string>("can't do this");
			var payloadedState = stateMachine.Enter<ExamplePayloadedState, int>(15);
			
			stateMachine.Update();
			
			Assert.AreEqual(15, payloadedState.Payload);
			Assert.IsTrue(stateMachine.IsActive(payloadedState));
			Assert.IsTrue(stateMachine.IsActive(typeof(ExamplePayloadedState)));
		}
	}
}