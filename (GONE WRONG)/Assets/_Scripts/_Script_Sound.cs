using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public readonly Vector3 position;
    public readonly float range;
    public Sound(Vector3 position, float range) 
    {
        position = this.position;
        range = this.range;
    }
}
