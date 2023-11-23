using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mode3KeyboardListener : MonoBehaviour
{
    public Button button7;
    public Button button8;
    public Button button9;

    public Button button4;
    public Button button5;
    public Button button6;

    public Button button1;
    public Button button2;
    public Button button3;

    public Button button0;

    public GameObject m3Drumpad;

    private HashSet<char> pressedKeys = new HashSet<char>();

    void Update()
    {
        if (m3Drumpad.activeInHierarchy && m3Drumpad.GetComponent<MonoBehaviour>().enabled)
        {
            foreach (char inputChar in Input.inputString)
            {
                if (char.IsDigit(inputChar))
                {
                    if (pressedKeys.Contains(inputChar))
                    {
                        // Key is already pressed, do nothing
                    }
                    else
                    {
                        // Key is pressed for the first time, invoke the corresponding button's onClick
                        pressedKeys.Add(inputChar);
                        switch (inputChar)
                        {
                            case '0':
                                button0.onClick.Invoke();
                                break;
                            case '1':
                                button1.onClick.Invoke();
                                break;
                            case '2':
                                button2.onClick.Invoke();
                                break;
                            case '3':
                                button3.onClick.Invoke();
                                break;
                            case '4':
                                button4.onClick.Invoke();
                                break;
                            case '5':
                                button5.onClick.Invoke();
                                break;
                            case '6':
                                button6.onClick.Invoke();
                                break;
                            case '7':
                                button7.onClick.Invoke();
                                break;
                            case '8':
                                button8.onClick.Invoke();
                                break;
                            case '9':
                                button9.onClick.Invoke();
                                break;
                        }
                    }
                }
            }

            // Check for released keys
            foreach (char key in new HashSet<char>(pressedKeys))
            {
                if (!Input.inputString.Contains(key.ToString()))
                {
                    // Key is released, remove it from the pressed keys
                    pressedKeys.Remove(key);
                }
            }
        }
    }
}
