using UnityEngine;

public class Sound
{
    public Sound(Vector3 _position, float _range) 
    {
        position = _position;
        range = _range;
    }

    public readonly Vector3 position;
    public readonly float range;
}
