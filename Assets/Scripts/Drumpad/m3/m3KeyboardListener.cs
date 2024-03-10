using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class m3KeyboardListener : MonoBehaviour
{
    public AudioSource[] audioSources;

    [SerializeField]
    private GameObject m3Drumpad;

    private HashSet<char> pressedKeys = new HashSet<char>(); // Hashset obsahující aktuálnì zmáèknuté klávesy

    void Update()
    {
        if (m3Drumpad.activeInHierarchy)
        {
            foreach (char inputChar in Input.inputString) // Pro každý stisknutý char za poslední frame
            {
                if (char.IsDigit(inputChar)) // Zkontroluje zda char je èíslo 0-9
                {
                    
                    int index = (int)(inputChar - '0'); // Pøevede na char na int
                    if (!pressedKeys.Contains(inputChar)) // Pokud klávesa není zmáèknutá, pøidá ji do pressedKeys a vykoná onClick
                    {
                        pressedKeys.Add(inputChar);
                        Debug.Log(inputChar);
                        audioSources[index].Play();
                    }
                }
            }

            // Udìlá kopii pressedKeys a projede ji     
            // (Kdyby jsme projeli pressedKeys a odebrali/pøidali prvek pøi projíždìní by byl Error)
            foreach (char key in new HashSet<char>(pressedKeys))
            {
                // Pokud klávesa již není zmáèknutá, odebere ji z pressedKeys
                if (!Input.inputString.Contains(key.ToString()))
                {
                    pressedKeys.Remove(key);
                }
            }
        }
    }
}
