using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitTextScript : MonoBehaviour
{
    [SerializeField] private float hitTextTime;
    [SerializeField] private TMP_Text text;


    public void StartHitText(CandyScript.CandyHitType candyHitType)
    {
        text.text = candyHitType.ToString();
        StartCoroutine(CreateAndDestroyHitText());
    }

    IEnumerator CreateAndDestroyHitText()
    {
        yield return new WaitForSeconds(hitTextTime);
        Destroy(gameObject);
    }

}
