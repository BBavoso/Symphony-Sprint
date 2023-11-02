using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace PauseMenuScript
{

    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu;
        public void Pause()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        public void Resume()
        {
            pauseMenu.setActive(false);
            Time.timeSacle 1f;
        }
        public void Home(int sceneID)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(sceneID);
        }



    }
}

