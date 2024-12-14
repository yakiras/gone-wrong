using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalPrompts : MonoBehaviour
{
    public bool journalOpen;
    public bool journalDisabled = false;

    public GameObject journalPageObj1;
    public GameObject journalPageObj2;
    public Sprite journalPrompt;
    public Sprite journalBook;

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !journalDisabled)
        {
            if (!journalOpen)
            {
                audioSource.PlayOneShot(sfxOpenBook);

                journalOverlay.enabled = true;
                journalOverlay.sprite = journalBook;
                journalPage1.enabled = true;
                journalPage2.enabled = true;
            }
            else
            {
                audioSource.PlayOneShot(sfxCloseBook);

                journalOverlay.enabled = false;
                journalPage1.enabled = false;
                journalPage2.enabled = false;
            }

            journalOpen = !journalOpen;
        }
    }
    public void PromptJournal(int newestEntry)
    {
        journalOverlay.sprite = journalPrompt;
        audioSource.PlayOneShot(sfxPenScribble);
        journalOverlay.enabled = true;
        currentEntryNum = newestEntry;
        if (newestEntry < 2)
            journalPage1.text += JournalEntryList[newestEntry];
        else
            journalPage2.text += JournalEntryList[newestEntry];
    }
}
