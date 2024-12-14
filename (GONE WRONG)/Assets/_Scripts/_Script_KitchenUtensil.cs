using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Script_KitchenUtensil : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip utensilSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("COLLISION DETECTED!!!");
            audioSource.PlayOneShot(utensilSound);
            float soundRange = 60f;
            var sound = new Sound(transform.position, soundRange);
            Sounds.MakeSound(sound);
        }
    }
}
