using System;
using NPG.States;

namespace NPG.States.Test
{
	public class SimplePayload
	{
		public string Name { get; }
		public int Level { get; }

		public SimplePayload(string name, int level)
		{
			Level = level;
			Name = name;
		}
	}
	
	public class SimplePayloadState : PayloadState<SimplePayload, StateMachine>
	{
		public string SelectedCharacterName { get; private set; }
		public int SelectedCharacterLevel { get; private set; }
		
		public SimplePayloadState(StateMachine stateMachine) : base(stateMachine)
		{
		}
		
		protected override void OnEnter(SimplePayload payload)
		{
			Console.WriteLine("SimplePayloadState:OnEnter");
			
			SelectedCharacterName = payload.Name;
			SelectedCharacterLevel = payload.Level;
		}

		protected override void OnExit()
		{
			Console.WriteLine("SimplePayloadState:OnExit");
			
			SelectedCharacterName = string.Empty;
			SelectedCharacterLevel = 0;
		}
	}
}