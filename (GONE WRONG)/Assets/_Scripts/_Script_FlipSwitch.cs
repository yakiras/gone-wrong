using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FlipSwitch : MonoBehaviour
{
    public float rotateSpeed = 0.03f;

    public GameObject lampsFlr1;
    public GameObject lampsFlr2;
    public GameObject lampsFlr3;
    public GameObject lampsSecurity;

    public Material unlitShade;
    public GameObject bgm;

    private AudioSource[] bgAudio;
    private bool isAnimating;
    private Quaternion targetRotation;

    private void Start()
    {
        isAnimating = false;
        targetRotation = transform.rotation;
        targetRotation.x += 179f;
        bgAudio = bgm.GetComponents<AudioSource>();
    }

    void Update()
    {
        if (isAnimating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
        }
    }

    public void Flip()
    {
        if (isAnimating)
            return;

        isAnimating = true;
        GetComponent<AudioSource>().Play();
        bgAudio[0].Stop();
        bgAudio[1].Stop();

        foreach (Transform parent in lampsSecurity.transform)
        {
            Transform lamp = parent.GetChild(0);

            lamp.GetChild(0).GetComponent<Renderer>().material = unlitShade;
            lamp.GetChild(1).GetComponent<Light>().enabled = false;
        }

        foreach (Transform parent in lampsFlr3.transform)
        {
            Transform lamp = parent.GetChild(0);

            lamp.GetChild(0).GetComponent<Renderer>().material = unlitShade;
            lamp.GetChild(1).GetComponent<Light>().enabled = false;
        }

        foreach (Transform parent in lampsFlr2.transform)
        {
            Transform lamp = parent.GetChild(0);

            lamp.GetChild(0).GetComponent<Renderer>().material = unlitShade;
            lamp.GetChild(1).GetComponent<Light>().enabled = false;
        }

        foreach (Transform parent in lampsFlr1.transform)
        {
            Transform lamp = parent.GetChild(0);

            lamp.GetChild(0).GetComponent<Renderer>().material = unlitShade;
            lamp.GetChild(1).GetComponent<Light>().enabled = false;
        }
    }
}
