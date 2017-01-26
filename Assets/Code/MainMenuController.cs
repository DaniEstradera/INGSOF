using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public GameObject PlayerCoop;
    public GameSettings Settings;
    public Toggle PlayerGamePad;
    public Toggle SecondPlayer;
    public Toggle SecondPlayerGamePad;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnStartPressed() {
        SceneManager.LoadScene(1);
    }
    public void On2PlayersPressed(bool SecondPlayer) {
        PlayerCoop.SetActive(SecondPlayer);
        SecondPlayerGamePad.interactable = SecondPlayer;
    }
    public void OnGamePadPressed(bool GamePad)
    {
        Settings.Player1GamePad = GamePad;
    }
    public void On2PlayerGamepadPressed(bool GamePad)
    {
        Settings.Player2GamePad = GamePad;
    }
    public void OnDualStickPressed(bool GamePad)
    {
        
        SecondPlayer.interactable = !GamePad;
        if (SecondPlayer.isOn){
            SecondPlayerGamePad.interactable = !GamePad;
            PlayerCoop.SetActive(!GamePad);
        }
        if (GamePad) { 
            PlayerCoop.SetActive(GamePad);
        } else if (!SecondPlayer.isOn) {
            PlayerCoop.SetActive(GamePad);
        }
        Settings.DualStick = GamePad;
    }
}
