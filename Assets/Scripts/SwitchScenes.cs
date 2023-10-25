using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public Image On;
    public Image Off;
    public GameObject Mode1;
    public GameObject Mode2;
    public bool lastMode;

    void Start()
    {

    }

    public void ON()
    {
        Off.gameObject.SetActive(true);
        On.gameObject.SetActive(false);
        Mode2.SetActive(true);
        Mode1.SetActive(false);
        lastMode = false;

        //false -> Mode2

    }

    public void OFF()
    {
        On.gameObject.SetActive(true);
        Off.gameObject.SetActive(false);
        Mode1.SetActive(true);
        Mode2.SetActive(false);
        lastMode = true;

        //true -> Mode1
    }
}
