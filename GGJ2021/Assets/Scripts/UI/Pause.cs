using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

	public GameObject pausePanel;
	private bool isPaused;

	void Update ()
    {
        if (Input.GetButtonDown ("Cancel") && !isPaused) 
		{
			DoPause();
		}
		else if (Input.GetButtonDown ("Cancel") && isPaused) 
		{
			UnPause ();
		}
	}

	public void DoPause()
	{
		isPaused = true;
		Time.timeScale = 0.0f;
        pausePanel.SetActive(true);
    }

	public void UnPause()
	{
		isPaused = false;
		Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
	}

    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Main");
        Application.Quit();
    }

}
