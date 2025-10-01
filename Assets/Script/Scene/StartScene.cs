using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip butClip;   
    public void NextScene()
    {
        audio.PlayOneShot(butClip);
        SceneManager.LoadScene("LobbyScene");
    }


    public void StartGame()
    {
        audio.PlayOneShot(butClip);
        Define.waveCount = 0;
        Define.gold = 300;
        Define.spawnCost = 10;
        Define.OnTowerCanvas = false;
        Define.gameClear = false;
        Define.gameOver = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void EndGame()
    {
        audio.PlayOneShot(butClip); 
        Application.Quit();
    }



}
