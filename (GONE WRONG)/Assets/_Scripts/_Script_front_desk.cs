using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Script_front_desk : MonoBehaviour
{
    public JournalPrompts script;
    private bool prompted = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !prompted)
        {
            script.PromptJournal(1);
            prompted = true;
        }
    }
}
