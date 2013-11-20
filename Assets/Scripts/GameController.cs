using UnityEngine;
using UnityEngine.WSA;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController SP;
	GameObject Player, AI;
	PlayerController playerController;
	AIController aiController;
	GUIText guiText;

	GUIText timerText;
	float roundTimer;

	enum GameState
	{
		Start,
		Playing,
		Won,
		Lost,
	};

	GameState CurrentState = GameState.Start;

	// Use this for initialization
	void Start () {
		SP = this;
		Player = GameObject.FindWithTag("Player");
		AI = GameObject.FindWithTag("AI");

		guiText = GameObject.Find("GUI Text").GetComponent<GUIText>();
		timerText = GameObject.Find("TimerText").GetComponent<GUIText>();

		playerController = Player.GetComponent<PlayerController>();
		aiController = AI.GetComponent<AIController>();

		roundTimer = 0.0f;

	}
	
	// Update is called once per frame
	void Update () 
	{
		switch(CurrentState)
		{
		case GameState.Start:

			guiText.text = "Click, Tap or Press a key to Start";

			if(Input.anyKey)
			{
				guiText.enabled = false;
				playerController.Active = true;
				aiController.Active = true;
				CurrentState = GameState.Playing;
			}
			break;
		case GameState.Playing:
			if(playerController.OutOfBounds)
			{
				CurrentState = GameState.Lost;
			}
			
			else if(aiController.OutOfBounds)
			{
				CurrentState = GameState.Won;
			}
			
			else
			{
				roundTimer += Time.deltaTime;
				timerText.text = "Round Timer: " + roundTimer.ToString("0.00");
			}
			break;
		case GameState.Won:

				guiText.enabled = true;
				guiText.text = "You Win!";
			SaveHighScore(roundTimer);
			if(Input.anyKey)
			{
				Reset();
			}

			break;
		case GameState.Lost:

				guiText.enabled = true;
				guiText.text = "You Lost!";
			if(Input.anyKey)
			{
				Reset();
			}
			break;
		
		}
	}

	public void paused()
	{
		Time.timeScale = 0.0f;
		guiText.enabled = true;
		guiText.text = "Paused!";
	}

	public void unpaused()
	{
		Time.timeScale = 1.0f;
		guiText.enabled = false;
	}

	void Reset()
	{
		roundTimer = 0.0f;
		playerController.OutOfBounds = false;
		aiController.OutOfBounds = false;
		Player.transform.rotation = new Quaternion(0,0,0,0);
		Player.transform.position = new Vector3(0.00375f,0.3530463f,-0.9551017f);
		playerController.Active = false;
		AI.transform.rotation = new Quaternion(0,0,0,0);
		AI.transform.position = new Vector3(0.00375f,0.3449954f,0.9198211f);
		aiController.Active = false;
		CurrentState = GameState.Start;
	}

	public void Quit()
	{
		UnityEngine.Application.Quit();
	}

	void SaveHighScore(float score)
	{
		
		float currentHighScore;
		if(PlayerPrefs.HasKey("HighScore"))
		{
			currentHighScore = PlayerPrefs.GetFloat("HighScore");
		}
		else
		{
			currentHighScore = 0.0f;
		}
		
		if(currentHighScore > score || currentHighScore == 0)
		{

			WindowsGateway.ShareHighScore();
			UpdateTile(score);
			PlayerPrefs.SetFloat("HighScore", score);	
		}
	}
	
	public float GetHighScore()
	{
		if(PlayerPrefs.HasKey("HighScore"))
		{
			return PlayerPrefs.GetFloat("HighScore");
		}
		else 
		{
			return 0.0f;
		}
	}

	public void UpdateTile(float score)
	{
		UnityEngine.WSA.Tile test = Tile.main; 
		test.Update("","","", "Best round time: " + score.ToString("0.00"));
	}
}
