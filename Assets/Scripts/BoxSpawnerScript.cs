using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawnerScript : MonoBehaviour
{

    [SerializeField] private GameObject boxPrefab;

    private void Start()
    {
        StartCoroutine(SpawnBoxes());
    }
    IEnumerator SpawnBoxes()
    {
        while (true)
        {
            SpawnBox();
            yield return new WaitForSeconds(1f);
        }
    }
    private void SpawnBox()
    {
        GameObject box1 = Instantiate(boxPrefab);
        int randNum = Random.Range(0, 5);
        PlayerScript.LineNumber lineNumber = PlayerScript.IntToLineNumber(randNum);

        float y = PlayerScript.LineNumberToYPosition(lineNumber);
        box1.transform.position = new Vector2(12, y);
    }
}
