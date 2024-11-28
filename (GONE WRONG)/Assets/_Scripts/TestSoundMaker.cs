using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundMaker : MonoBehaviour
{
    [SerializeField] private float soundRange = 25f;

    private void OnMouseDown()
    {
        var sound = new Sound(transform.position, soundRange);
        print($"Sound at pos {sound.position} created");
        Sounds.MakeSound(sound);
        
    }
}
