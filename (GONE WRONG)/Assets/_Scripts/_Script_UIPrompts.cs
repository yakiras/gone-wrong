using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPrompts : MonoBehaviour
{
    public float tutorialLength = 4f;
    public bool tutorialPlaying;
    public GameObject textPromptObj;
    public JournalPrompts journalScript;

    private TextMeshProUGUI textPrompt;
    private Image uiOverlay;

    public Sprite promptWASD;
    public Sprite promptShift;

    public Sprite promptE;
    public Sprite promptThrow;

    public Sprite promptSpace;
    public Sprite promptR;

    void Start()
    {
        uiOverlay = GetComponent<Image>();
        uiOverlay.enabled = false;
        textPrompt = textPromptObj.GetComponent<TextMeshProUGUI>();
        textPrompt.enabled = false;

        StartCoroutine(StartTutorial());
        tutorialPlaying = true;
    }

    IEnumerator StartTutorial()
    {
        // JOURNAL PROMPT + NEW ENTRY //
        yield return new WaitForSeconds(3);
        journalScript.PromptJournal(0);

        // WASD PROMPT //
        yield return new WaitUntil(() => (journalScript.journalOpen));
        yield return new WaitUntil(() => (!journalScript.journalOpen));
        journalScript.journalDisabled = true;
        uiOverlay.enabled = true;
        uiOverlay.sprite = promptWASD;

        // SHIFT PROMPT //
        yield return new WaitForSeconds(tutorialLength);
        uiOverlay.sprite = promptShift;

        // SPACE PROMPT //
        yield return new WaitForSeconds(tutorialLength);
        uiOverlay.sprite = promptSpace;

        // FINISH TUTORIAL //
        yield return new WaitForSeconds(tutorialLength);
        uiOverlay.enabled = false;
        journalScript.journalDisabled = false;

        tutorialPlaying = false;
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

    public void PromptClear()
    {
        uiOverlay.enabled = false;
    }

    public IEnumerator DisplayText(string prompt)
    {
        textPrompt.enabled = true;
        textPrompt.text = prompt;
        yield return new WaitForSeconds(3);
        textPrompt.enabled = false;
    }
}