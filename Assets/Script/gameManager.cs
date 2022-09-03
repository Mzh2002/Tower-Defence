using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{

    public GameObject endUI;
    public Text endText;

    public static gameManager Instance;

    private enemySpawner spawner;

    private void Start()
    {
        Instance = this;
        spawner = GetComponent<enemySpawner>();
    }

    public void win()
    {
        endUI.SetActive(true);
        endText.text = "YOU WIN";
        endText.color = Color.yellow;
    }

    public void fail()
    {
        spawner.Stop();
        endUI.SetActive(true);
        endText.text = "GAME OVER";
        endText.color = Color.white;
    }

    public void onReplayButtonDown()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onMenuButtonDown()
    {
        SceneManager.LoadScene(0);
    }
}
