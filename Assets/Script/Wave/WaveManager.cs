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

        //StartCoroutine(WaveSet());
        Debug.Log($"[WM.Start] DTM.WaveTableData={(DataTableManager.WaveTableData == null ? "NULL" : "not null")}");
        waveTable = DataTableManager.WaveTableData;
        Debug.Log($"[WM.Start] waveTable={(waveTable == null ? "NULL" : "not null")}");
        if (waveTable == null)
        {
            Debug.LogError("[WM] waveTable null → 초기화/순서 문제");
            return;
        }
        StartCoroutine(WaveSet());
    }
    

    private bool waveClearSkip = false;

    private bool waveActive = false;    

   

    IEnumerator WaveSet() // 웨이브 단계
    {
        Debug.Log($"[WM.WaveSet] waveTable null? {(waveTable == null)}, count={(waveTable == null ? -1 : waveTable.Count)}");
        for (int i = 0; i < waveTable.Count; i++)
        {

            var wd = waveTable.GetByIndex(i);
            if (wd == null)
            {
                Debug.LogError($"[WM.WaveSet] waveTable.Get({i}) == null (데이터 테이블에서 {i}번째 웨이브가 비어있음)");
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
    

        timer += wd.waveTime; //치트때문에 더함 나중에 수정해야함 
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
                yield break;
            }

            //timer -= Time.deltaTime;

            if (enemyTotalCount <= 0)
            {
               
                yield break;
            }

            if (timer <= 0f)
            {
               
                waveActive = false; 
                Time.timeScale = 0f;
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
}
