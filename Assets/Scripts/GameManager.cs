using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum GameState {Ingame , Menu}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool DoorOpen;
    public GameObject WinPanel;
    public GameObject LosePanel;
    public GameState GameState;


    private int playerCount = 0;
    private void Awake()
    {
        instance = this;
    }
    public void Update()
    {
        if(playerCount >= 2 && GameState != GameState.Menu)
        {
            WinPlayer();
        }
    }

    public void WinPlayer()
    {
        int lastNumber = int.Parse(SceneManager.GetActiveScene().name[SceneManager.GetActiveScene().name.Length - 1].ToString());
        SceneManager.LoadScene("Level" + (lastNumber + 1).ToString());
        GameState = GameState.Menu;

    }

    public void LosePlayer()
    {
        LosePanel.SetActive(true);
        GameState = GameState.Menu;
        
    }

    public void PlayerIn()
    {
        playerCount += 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
