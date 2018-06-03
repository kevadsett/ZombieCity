using UnityEngine;

namespace GameStateMachine
{
	public class IntroGameState : AbstractGameState
	{
		private StateMachine stateMachine;

		public IntroGameState(StateMachine stateMachine, GameObject screen) : base (screen)
		{
			this.stateMachine = stateMachine;
			CityGenerator.Instance.Generate();
		}

		public override void Update()
		{
			if (Input.anyKeyDown)
			{
				stateMachine.ChangeState(StateMachine.GameState.Running);
			}
		}
	}
}
