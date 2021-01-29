using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject titlePanel;
    public GameObject selectionPanel;

    public void StartGame()
    {
        StartCoroutine(BeginGame(1.1f));
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Main");
        Application.Quit();
        Debug.Log("Game quit!");
    }

    IEnumerator BeginGame(float waitTime)
    {
        for (float i = 0; i <= .25; i += Time.deltaTime)
        {
            yield return null;
        }
        titlePanel.SetActive(false);
        selectionPanel.SetActive(false);
        SceneManager.LoadScene("Main");
    }	
}
