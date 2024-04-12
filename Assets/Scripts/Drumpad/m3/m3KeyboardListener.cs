using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M3KeyboardListener : MonoBehaviour
{
    public AudioSource[] audioSources;

    [SerializeField]
    private GameObject m3Drumpad;


    [SerializeField]
    private Image m3logo;

    [SerializeField]
    private Image Background;


    void Start()    // Zmìna barvy loga
    {

        m3logo.color = Background.color;
    }






    private HashSet<char> pressedKeys = new(); // Hashset obsahující aktuálnì zmáèknuté klávesy

    void Update()
    {
        string currentInput = Input.inputString;
        foreach (char inputChar in currentInput)
        {
            if (char.IsDigit(inputChar))
            {
                int index = inputChar - '0';
                if (index >= 0 && index < M3DropReceiver.audioClips.Length && !pressedKeys.Contains(inputChar))
                {
                    pressedKeys.Add(inputChar);
                    if (M3DropReceiver.audioClips[index] != null)
                    {
                        AudioSource source = M3DropReceiver.audioSources[index];
                        source.clip = M3DropReceiver.audioClips[index];
                        source.Play();
                    }
                }
            }
        }

        pressedKeys.RemoveWhere(key => !currentInput.Contains(key));
    }

}
