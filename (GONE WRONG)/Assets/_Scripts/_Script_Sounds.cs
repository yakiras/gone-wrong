using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static void MakeSound(Sound sound) 
    {
        Collider[] col = Physics.OverlapSphere(sound.position, sound.range);

        for (int i = 0; i < col.Length; i++)
            if (col[i].TryGetComponent(out _Script_SoundMonsterAI monster))
                monster.ReactToSound(sound); 
    }
}
