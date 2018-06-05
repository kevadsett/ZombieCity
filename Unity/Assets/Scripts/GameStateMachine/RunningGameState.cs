using UnityEngine;

namespace GameStateMachine
{
	public class RunningGameState : AbstractGameState
	{
		private StateMachine stateMachine;

		private GameObject player;

		public RunningGameState(StateMachine stateMachine, GameObject screen, GameObject player) : base (screen)
		{
			this.stateMachine = stateMachine;
			this.player = player;
			PlayerHealth.OnHealthLost += PlayerHealthOnOnHealthLost;
			player.SetActive(true);
            player.GetComponentInChildren<PlayerHealth>().ResetHealth();

            var dayNight = GameObject.FindObjectOfType<DayNightController>();
            dayNight.enabled = true;
			dayNight.ResetDay();

            GameObject.FindObjectOfType<WeaponStorage>().ResetAmmo();
            GameObject.FindObjectOfType<WeaponSelector>().ResetWeapon();
            GameObject.FindObjectOfType<AmmoTextUpdater>().UpdateText();
		}

		private void PlayerHealthOnOnHealthLost(int newhealth)
		{
			if (newhealth == 0)
			{
				AudioPlayer.PlaySound ("GameOver");
				stateMachine.ChangeState(StateMachine.GameState.GameOver);
			}
		}

		public override void Destroy()
		{
			base.Destroy();
			PlayerHealth.OnHealthLost -= PlayerHealthOnOnHealthLost;
			player.SetActive(false);
		}
	}
}
