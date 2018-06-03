using UnityEngine;

namespace GameStateMachine
{
	public abstract class AbstractGameState : IGameState
	{
		protected GameObject HudObject;

		protected AbstractGameState(GameObject screen)
		{
			HudObject = screen;
			ShowScreen(true);
		}

		protected void ShowScreen(bool show)
		{
			HudObject.SetActive(show);
		}

		public virtual void Update()
		{
			// nothing to do here
		}

		public virtual void Destroy()
		{
			ShowScreen(false);
		}
	}
}
