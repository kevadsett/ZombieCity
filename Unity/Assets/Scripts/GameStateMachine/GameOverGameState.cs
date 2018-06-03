using UnityEngine;

namespace GameStateMachine
{
	public class GameOverGameState : AbstractGameState
	{
		private StateMachine stateMachine;

		private float startTime;

		private GameObject gameOverScreen;

		public GameOverGameState(StateMachine stateMachine, GameObject screen) : base (screen)
		{
			startTime = Time.time;
			this.stateMachine = stateMachine;
			ShowScreen(true);
			var controlLight = GameObject.Find("ControllableLight");
			controlLight.GetComponent<LightingControl>().enabled = true;
			controlLight.GetComponent<DayNightController>().Reset();
		}

		public override void Update()
		{
			if (Time.time - startTime < 2f) return;

			if (Input.anyKeyDown)
			{
				stateMachine.ChangeState(StateMachine.GameState.Intro);
			}
		}
	}
}
