using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }


    public void StartGame()
    {
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
        Application.Quit();
    }



}
