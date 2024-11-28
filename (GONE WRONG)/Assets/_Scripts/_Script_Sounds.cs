using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sounds
{
    public static void MakeSound(Sound sound) 
    {
        Debug.Log($"Sound: with pos {sound.position} and range {sound.range} created");
        Collider[] col = Physics.OverlapSphere(sound.position, sound.range);

        for (int i = 0; i < col.Length; i++)
            if (col[i].TryGetComponent(out _Script_SoundMonsterAI monster))
            {
                Debug.Log("Monster heard sound");
                monster.ReactToSound(sound);
            }
    }
}
