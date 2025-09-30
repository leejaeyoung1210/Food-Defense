using System.Collections;
using System.Net.NetworkInformation;
using System.Xml;
using UnityEngine;




public class WaveManager : MonoBehaviour
{
    private WaveTable waveTable;
    public float timer { get; private set; }
    public EnemySpawner enemySpawner;


    public static int enemyTotalCount = 0;

    void Start()
    {
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
    {
        for (int i = 0; i < waveTable.Count; i++)
        {
            var wd = waveTable.GetByIndex(i);
            if (wd == null)
            {
                continue; // 혹은 yield break
            }            
            waveActive = true;
            Define.waveCount++;

            yield return StartCoroutine(OnWave(wd));
            yield return new WaitForSeconds(1f);
        }
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
                    enemySpawner.Spawn(enemy, enemySpawner.transform.position);
                    enemyTotalCount++;
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
