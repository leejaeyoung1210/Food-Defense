using System.Collections;
using System.Net.NetworkInformation;
using System.Xml;
using UnityEngine;




public class WaveManager : MonoBehaviour
{
    private WaveTable waveTable;
    public float timer { get; private set; }
    public EnemySpawner enemySpawner;

    public EnemySpawner enemySpawner2;

    public UIManager uiManager; 

    public static int enemyTotalCount = 0;

    public GameObject warningUi;

    public StageManager stageManager;
  

    private  void Start()
    {
        enemySpawner2.gameObject.SetActive(false);
        waveTable = DataTableManager.WaveTableData;
        enemyTotalCount = 0;
        if (waveTable == null)
        {
            return;
        }
        StartCoroutine(WaveSet());
    }


    private bool waveClearSkip = false;

    private bool waveActive = false;



    IEnumerator WaveSet() // 웨이브 단계
    {  // 테스트용 
        Define.gold = 100000000;        
        for (int i = 0; i < waveTable.Count; i++)
        {
            var wd = waveTable.GetByIndex(i);
            Debug.Log($"웨이브 {wd.Id} : {wd.Name}"); // 테스트용
            if (wd == null)
            {
                continue; // 혹은 yield break
            }
            if ((i == 5||i==10) && i != 0)
            {
                stageManager.LoadStage(i / 5); // 0,1,2,3...
            }


            yield return StartCoroutine(WaveCount());

            waveActive = true;
            Define.waveCount++;
            if (Define.waveCount>10)
            {
                enemySpawner2.gameObject.SetActive(true);
            }

            yield return StartCoroutine(OnWave(wd));
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator WaveCount()
    {
        warningUi.SetActive(false);
        uiManager.SetTimerTextColor(Color.yellow);  
        timer = 3f;        
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;            
        }
        uiManager.SetTimerTextColor(Color.white);
        //yield return new WaitForSeconds(1f);
    }

    IEnumerator OnWave(WaveData wd) // 웨이브 플레이
    {
        timer = wd.waveTime;
        Define.gold += wd.bonusCoin;
        foreach (var slot in wd.slots)
        {

            for (int i = 0; i < slot.count; i++)
            {

                var enemy = slot.enemyData;
                if (enemy != null)
                {
                    enemySpawner.Spawn(enemy, enemySpawner.transform.position,false);
                    enemyTotalCount++;

                    if (enemySpawner2.gameObject.activeSelf)
                    {
                        enemySpawner2.Spawn(enemy, enemySpawner2.transform.position,true);
                        enemyTotalCount++;
                    }
                    
                }
                yield return new WaitForSeconds(1f);
            }

        }

        while (true)
        {
            if (waveClearSkip)
            {
                waveClearSkip = false;
                timer += 100f;
                yield break;
            }

            //timer -= Time.deltaTime;

            if (enemyTotalCount <= 0)
            {
                waveActive = false;
                if (Define.waveCount > waveTable.Count) //클리어 조건
                {
                    GameClear();
                }
                yield break;
            }

            if (timer <= 0f)
            {
                waveActive = false;
                GameOver();
            }
            yield return null;
        }
    }

    private void Update()
    {
        if (waveActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 5f)
            {
                warningUi.SetActive(true);
            }            
        }
    }

    public void WaveClearSkip()
    {
        waveClearSkip = true;
    }

    private void GameClear()
    {
        Time.timeScale = 0f;
        Define.gameClear = true;
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        Define.gameOver = true;
    }




}
