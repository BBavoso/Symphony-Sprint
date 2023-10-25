using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitTextScript : MonoBehaviour
{
    [SerializeField] private float hitTextTime;
    [SerializeField] private TMP_Text text;

    [SerializeField] private float animationXDistance;
    [SerializeField] private float animationYDistance;

    private float timePassed;


    private void Start()
    {
        timePassed = 0;
        // Debug.Log(text.material.color.a);
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        FadeText();
        HandleAnimation();
    }

    private void FadeText()
    {
        float newAlpha = Mathf.Lerp(1f, 0f, timePassed / hitTextTime);
        // Debug.Log($"timePassed: {timePassed} newAlpha: {newAlpha}");
        text.faceColor = new Color(
            text.faceColor.r,
            text.faceColor.g,
            text.faceColor.b,
            newAlpha
        );
    }
    public void StartHitText(CandyScript.CandyHitType candyHitType)
    {
        text.text = candyHitType.ToString();
        StartCoroutine(CreateAndDestroyHitText());
    }

    void HandleAnimation()
    {
        float percentPassed = timePassed / hitTextTime;

        float xMoveDisance = animationXDistance / hitTextTime / 100;
        float yMoveDistance = (-2 * percentPassed + 1) * animationYDistance / 100; // Derivative of -x^2 + x

        transform.position += new Vector3(xMoveDisance, yMoveDistance);
    }

    IEnumerator CreateAndDestroyHitText()
    {
        yield return new WaitForSeconds(hitTextTime);
        Destroy(gameObject);
    }

}
