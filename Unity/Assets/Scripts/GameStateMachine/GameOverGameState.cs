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
            // turn off the day/night, this stops the day continuing
            var dayNight = GameObject.FindObjectOfType<DayNightController>();
            dayNight.ResetDay();
            dayNight.enabled = false;

            // turn on the intro camera:
            GameObject.Find("IntroCamera").GetComponent<AudioListener>().enabled=true;
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
