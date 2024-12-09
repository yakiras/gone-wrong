using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPrompts : MonoBehaviour
{
    public GameObject journalEntryObj;
    public AudioClip sfxPenScribble;
    public AudioClip sfxOpenBook;
    public AudioClip sfxCloseBook;

    private AudioSource audioSource;
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
        audioSource = transform.parent.parent.GetComponent<AudioSource>();

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
                audioSource.PlayOneShot(sfxOpenBook);

                uiOverlay.enabled = true;
                uiOverlay.sprite = journalOverlay;
                currentEntryText.enabled = true;
                currentEntryText.text = JournalEntryList[currentEntryNum];
            }
            else
            {
                audioSource.PlayOneShot(sfxCloseBook);

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
        audioSource.PlayOneShot(sfxPenScribble);

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

    public IEnumerator PromptR()
    {
        uiOverlay.sprite = promptR;
        uiOverlay.enabled = true;
        yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.R) || Time.time > 5));
        uiOverlay.enabled = false;
    }
    
    public void PromptE()
    {
        uiOverlay.sprite = promptE;
        uiOverlay.enabled = true;
    }

    public void PromptJournal()
    {
        currentEntryNum++;
        uiOverlay.sprite = promptJournal;
        audioSource.PlayOneShot(sfxPenScribble);
        uiOverlay.enabled = true;
    }

    public void PromptClear()
    {
        uiOverlay.enabled = false;
    }
}