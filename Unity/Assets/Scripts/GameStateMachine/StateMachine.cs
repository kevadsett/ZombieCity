using System.Collections;
using System.Collections.Generic;
using GameStateMachine;
using UnityEngine;

public class StateMachine : MonoBehaviour {

	public enum GameState
	{
		Intro,
		Running,
		GameOver
	}

	public GameObject IntroScreen;
	public GameObject RunningScreen;
	public GameObject GameOverScreen;

	public GameObject Player;

	private IGameState currentState;

	public static GameState CurrentState { get; private set; }

	void Start ()
	{
		ChangeState(GameState.Intro);
	}
	
	// Update is called once per frame
	void Update () {
		if (currentState == null) return;

		currentState.Update();
	}

	public void ChangeState(GameState newState)
	{
		if (currentState != null)
		{
			currentState.Destroy();
		}

		CurrentState = newState;

		switch (newState)
		{
			case GameState.Intro:
				currentState = new IntroGameState(this, IntroScreen);
				break;
			case GameState.Running:
				currentState = new RunningGameState(this, RunningScreen, Player);
				break;
			case GameState.GameOver:
				currentState = new GameOverGameState(this, GameOverScreen);
				break;
		}
	}
}
