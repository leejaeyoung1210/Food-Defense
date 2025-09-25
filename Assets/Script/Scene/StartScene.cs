using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] private Button startBut;
    [SerializeField] private Button endBut;


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
