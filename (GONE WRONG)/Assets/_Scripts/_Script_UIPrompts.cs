using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPrompts : MonoBehaviour
{
    public GameObject journalEntryObj;
    private TextMeshProUGUI currentEntryText;
    private int currentEntryNum = 0;
    private bool journalOpen;
    private bool journalDisabled = false;

    private Image uiOverlay;

    public Sprite promptWASD;
    public Sprite promptShift;

    public Sprite promptE;
    public Sprite promptThrow;

    public Sprite promptSpace;
    public Sprite promptR;

    public Sprite promptJournal;
    public Sprite journalOverlay;

    private string[] JournalEntryList = { "This is Entry #1.",
                                          "This is Entry #2.",
                                          "This is Entry #3.", };

    void Start()
    {
        uiOverlay = GetComponent<Image>();
        uiOverlay.enabled = false;
        currentEntryText = journalEntryObj.GetComponent<TextMeshProUGUI>();
        currentEntryText.enabled = false;
        journalOpen = false;

        StartCoroutine(StartTutorial());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !journalDisabled)
        {
            if (!journalOpen)
            {
                uiOverlay.enabled = true;
                uiOverlay.sprite = journalOverlay;
                currentEntryText.enabled = true;
                currentEntryText.text = JournalEntryList[currentEntryNum];
            }
            else
            {
                uiOverlay.enabled = false;
                currentEntryText.enabled = false;
            }

            journalOpen = !journalOpen;
        }
    }

    IEnumerator StartTutorial()
    {
        // JOURNAL PROMPT + NEW ENTRY //
        yield return new WaitForSeconds(3);
        uiOverlay.sprite = promptJournal;
        uiOverlay.enabled = true;
        // TO-DO: pen scribble audio

        // WASD PROMPT //
        yield return new WaitUntil(() => (journalOpen));
        yield return new WaitUntil(() => (!journalOpen));
        journalDisabled = true;
        uiOverlay.enabled = true;
        uiOverlay.sprite = promptWASD;

        // SHIFT PROMPT //
        yield return new WaitForSeconds(5);
        uiOverlay.sprite = promptShift;

        // SPACE PROMPT //
        yield return new WaitForSeconds(5);
        uiOverlay.sprite = promptSpace;

        // FINISH TUTORIAL //
        yield return new WaitForSeconds(5);
        uiOverlay.enabled = false;
        journalDisabled = false;
    }

    public void PromptR()
    {
        uiOverlay.sprite = promptR;
    }
    
    public void PromptE()
    {
        uiOverlay.sprite = promptE;
    }

    public void PromptJournal()
    {
        currentEntryNum++;
        uiOverlay.sprite = promptJournal;
        // pen scribble audio
    }

}