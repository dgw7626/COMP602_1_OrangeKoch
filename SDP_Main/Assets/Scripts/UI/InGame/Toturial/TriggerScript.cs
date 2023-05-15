using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TriggerScript : MonoBehaviour
{
    public TextMeshProUGUI MoveText;
    public TextMeshProUGUI JumpText;

    public TextMeshProUGUI SprintText;
    public GameObject trigger1;
    public GameObject trigger2;

    private bool trigger1Activated = false;
    private bool trigger2Activated = false;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered!");

        if (!trigger1Activated && other.CompareTag("Player") && trigger1.CompareTag("Player"))
        {
            Debug.Log("Trigger1 entered!");

            MoveText.rectTransform.anchoredPosition -= new Vector2(0, 50);


            JumpText.gameObject.SetActive(true);


            trigger1.SetActive(false);


            trigger2.SetActive(true);

            trigger1Activated = true;
        }
        else if (!trigger2Activated && other.CompareTag("Player") && other.gameObject == trigger2)
        {

            MoveText.rectTransform.anchoredPosition -= new Vector2(0, 50);
            JumpText.rectTransform.anchoredPosition -= new Vector2(0, 50);

            SprintText.gameObject.SetActive(true);

            trigger2.SetActive(false);

            trigger2Activated = true;
        }
    }
}
