using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using PopupWindow = UnityEditor.PopupWindow;
using TMPro;
using UnityEditor.MPE;

public class StartScript : MonoBehaviour
{

    public bool changingHit;
    public bool changingJump;


    public void Start()
    {
        changingHit = false;
        changingJump = false;
    }

    public void Update()
    {
        if (changingHit && !changingJump)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    ChangeKey(keyCode);
                }
            }
        }
        if (changingJump && !changingHit)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    ChangeKey(keyCode);
                }
            }
        }
    }

    private void ChangeKey(KeyCode key)
    {
        if (changingHit)
        {
            if (Player.jumpKey != key)
            {
                Player.hitKey = key;
                GameObject.FindGameObjectWithTag("HitInput").GetComponent<TMP_Text>().text = key.ToString();
            }
            changingHit = false;
            GameObject.FindGameObjectWithTag("KeyMessage").SetActive(false);
        }
        if (changingJump)
        {
            if (Player.hitKey != key)
            {
                Player.jumpKey = key;
                GameObject.FindGameObjectWithTag("JumpInput").GetComponent<TMP_Text>().text = key.ToString();
            }
            changingJump = false;
            GameObject.FindGameObjectWithTag("KeyMessage").SetActive(false);
        }
    }

    public void PlayOnClick()
    {
        SceneManager.LoadScene("CutScene");
    }

    public void QuitOnClick()
    {
        Application.Quit();
    }

    public void ControlOnClick()
    {
        TMP_Text hitInput = GameObject.FindGameObjectWithTag("HitInput").GetComponent<TMP_Text>();
        TMP_Text jumpInput = GameObject.FindGameObjectWithTag("JumpInput").GetComponent<TMP_Text>();
        KeyCode[] controls = Player.GetControls();
        hitInput.text = controls[0].ToString();
        jumpInput.text = controls[1].ToString();
    }

    public void ChangeHitOnClick()
    {
        changingHit = true;
    }

    public void ChangeJumpOnClick()
    {
        changingJump = true;
    }
}