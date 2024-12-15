using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class JournalPrompts : MonoBehaviour
{
    public bool journalOpen;
    public bool mapOpen;
    public bool journalDisabled = false;
    public bool journalPrompted;

    public GameObject journalPageObj1;
    public GameObject journalPageObj2;
    public Sprite journalPrompt;
    public Sprite journalBook;
    public Sprite mapPage;

    public Sprite promptF;
    public Sprite promptR;
    public UIPrompts uiScript;

    public AudioClip sfxPenScribble;
    public AudioClip sfxOpenBook;
    public AudioClip sfxCloseBook;
    private AudioSource audioSource;
    private Image journalOverlay;

    private TextMeshProUGUI journalPage1;
    private TextMeshProUGUI journalPage2;
    private int currentEntryNum = 0;

    private string[] JournalEntryList = { "09/10/2013\n\nForget the views, this place is way worse than I remember seeing online. Nobody in the lobby when I get out of the elevator, " +
            "and better yet the damn thing’s not working when I turn around! Not to mention the weird shuffling noises I’m hearing down the hall. Fuck this, I’m out of " +
            "here… After I figure out how to get the damn elevator working again. Maybe there’s a manual or something behind the help desk?\n\n",

                                          "So apparently, according to the employee handbook hiding beneath the counter under a couple inches of dust, there’s a switch " +
            "in the security office on the third floor that will turn off the hotel’s power and divert it to the elevator. But it also says that the door is kept locked" +
            ", so the only way in would be calling a janitor. It does mention something later about a janitor’s closet in the kitchen…\n\n",

                                          "Place must be pretty old to keep using keys for everything instead of keycards. I’ve just gotta take this up to the third floor," +
            " flip the power, and get the hell out of here. There’s also some notes here between coworkers. Something about staff going missing, then some showing up and " +
            "acting weird. The notes don’t seem that old either…\n\n",

                                          "So I hit the switch, grab the flashlight, and book it downstairs to the elevator. Given how nice it is in here compared to the " +
            "rest of the hotel, I’d guess it’s the only room used regularly. By who? I’d prefer to live than find out.\n\n"
    };

    void Start()
    {
        audioSource = transform.parent.parent.GetComponent<AudioSource>();

        journalOverlay = GetComponent<Image>();
        journalOverlay.enabled = false;

        journalPage1 = journalPageObj1.GetComponent<TextMeshProUGUI>();
        journalPage2 = journalPageObj2.GetComponent<TextMeshProUGUI>();
        journalPage1.enabled = false;
        journalPage2.enabled = false;

        journalOpen = false;
        journalPrompted = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !journalDisabled)
        {
            if (!journalOpen)
            {
                journalPrompted = true;
                audioSource.PlayOneShot(sfxOpenBook);

                journalOverlay.enabled = true;
                journalOverlay.sprite = journalBook;
                journalPage1.enabled = true;
                journalPage2.enabled = true;
                journalOpen = true;

                journalPrompted = false;
                uiScript.ClearText();
            }
            else
            {
                journalPrompted = false;
                audioSource.PlayOneShot(sfxCloseBook);

                journalOverlay.enabled = false;
                journalPage1.enabled = false;
                journalPage2.enabled = false;
                journalOpen = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.M) && !journalDisabled)
        {
            if (!mapOpen)
            {
                audioSource.PlayOneShot(sfxOpenBook);

                journalOverlay.enabled = true;
                journalOverlay.sprite = mapPage;
                mapOpen = true;
                uiScript.ClearText();
            }
            else
            {
                audioSource.PlayOneShot(sfxCloseBook);

                journalOverlay.enabled = false;
                mapOpen = false;
            }
        }
    }
    public void PromptJournal(int newestEntry)
    {
        journalPrompted = true;
        journalOverlay.sprite = journalPrompt;
        audioSource.PlayOneShot(sfxPenScribble);
        journalOverlay.enabled = true;
        currentEntryNum = newestEntry;
        journalPage1.text = "";
        journalPage2.text = "";
        for (int i = 0; i <= newestEntry; i++)
        {
            if (i < 2)
                journalPage1.text += JournalEntryList[i];
            else
                journalPage2.text += JournalEntryList[i];
        }
    }

    public IEnumerator PromptF()
    {
        journalPrompted = true;
        journalOverlay.sprite = promptF;
        journalOverlay.enabled = true;
        yield return new WaitForSeconds(5);
        journalOverlay.enabled = false;
        journalPrompted = false;
    }

    public IEnumerator PromptR()
    {
        journalPrompted = true;
        journalOverlay.sprite = promptR;
        journalOverlay.enabled = true;
        yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.R) || Time.time > 5000));
        journalOverlay.enabled = false;
        journalPrompted = false;
    }
}
