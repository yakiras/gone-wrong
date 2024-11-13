using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Script_doorHinge : MonoBehaviour
{

    public bool version1 = true;
    public bool angleOveride = false;

    public bool isOpen = false;
    public float openChange;
    public float closeChange;

    void Start()
    {
        if (version1)
        {
            openChange = 90f;
            closeChange = 0f;
        }
        else if (angleOveride)
        {
            openChange = openChange;
            closeChange = closeChange;
        }
        else
        {
            openChange = 0f;
            closeChange = -90f;
        }
    }
        
}
