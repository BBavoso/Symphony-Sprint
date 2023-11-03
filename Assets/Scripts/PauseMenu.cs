using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace PauseMenuScript
{

    public class PauseMenu : MonoBehaviour
    {

        [SerializeField] GameObject pauseMenu;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Pause();
            }
        }

        private void Pause()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        public void Home(string scene)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(scene);
        }



    }
}

