using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TransitionScene());
    }

    IEnumerator TransitionScene()
    {
        yield return new WaitForSeconds(29.0f);


        SceneManager.LoadScene("FirstLevel");
    }
}
