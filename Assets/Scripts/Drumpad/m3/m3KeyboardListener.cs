using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class m3KeyboardListener : MonoBehaviour
{
    public AudioSource[] audioSources;

    [SerializeField]
    private GameObject m3Drumpad;

    private HashSet<char> pressedKeys = new HashSet<char>(); // Hashset obsahuj�c� aktu�ln� zm��knut� kl�vesy

    void Update()
    {
        if (m3Drumpad.activeInHierarchy)
        {
            foreach (char inputChar in Input.inputString) // Pro ka�d� stisknut� char za posledn� frame
            {
                if (char.IsDigit(inputChar)) // Zkontroluje zda char je ��slo 0-9
                {
                    
                    int index = (int)(inputChar - '0'); // P�evede na char na int
                    if (!pressedKeys.Contains(inputChar)) // Pokud kl�vesa nen� zm��knut�, p�id� ji do pressedKeys a vykon� onClick
                    {
                        pressedKeys.Add(inputChar);
                        Debug.Log(inputChar);
                        audioSources[index].Play();
                    }
                }
            }

            // Ud�l� kopii pressedKeys a projede ji     
            // (Kdyby jsme projeli pressedKeys a odebrali/p�idali prvek p�i proj�d�n� by byl Error)
            foreach (char key in new HashSet<char>(pressedKeys))
            {
                // Pokud kl�vesa ji� nen� zm��knut�, odebere ji z pressedKeys
                if (!Input.inputString.Contains(key.ToString()))
                {
                    pressedKeys.Remove(key);
                }
            }
        }
    }
}
