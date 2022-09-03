using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{

    public static int enemyAlive = 0;
    public wave[] waves;
    public Transform START;
    public float waveRate = 1;
    private Coroutine coroutine;

    private void Start()
    {
        coroutine = StartCoroutine(SpawnEnemy());
    }

    public void Stop()
    {
        StopCoroutine(coroutine);
    }

    IEnumerator SpawnEnemy()
    {
        foreach(wave Wave in waves){
            for(int i=0; i<Wave.count; i++)
            {
                GameObject.Instantiate(Wave.enemyPrefab, START.position, Quaternion.identity);
                enemyAlive++;
                yield return new WaitForSeconds(Wave.rate);
            }
            while (enemyAlive > 0)
            {
                yield return 0;
            }
            yield return new WaitForSeconds(waveRate);
        }

        while (enemyAlive > 0)
        {
            yield return 0;
        }

        gameManager.Instance.win();
    }
}
