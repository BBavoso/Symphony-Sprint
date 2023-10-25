using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour {

    public void Start() {
       
    }

    public void PlayOnClick() {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitOnClick() {
        Application.Quit();
    }
}