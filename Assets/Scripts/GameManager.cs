using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;   

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject mode1;
    [SerializeField] private GameObject defaultMode2Row;

    public Button[] m1Buttons = new Button[64];
    public List<List<Button>> mode2Rows;

    public List<List<AudioClip>> Beats = new List<List<AudioClip>>(); // Initialize Beats list

    public static GameManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Toto je jedin� instance gameManageru, p�i�a��me do Instance prom�nn�
            Instance = this;
        }


        InitializeBeatsList();

        InitializeM1ButtonArray();

        mode2Rows = new List<List<Button>>(); // Zalo�� list pro M2 �ady

        InitializeM2Row(defaultMode2Row);

       

    }


    private void InitializeBeatsList()
    {
        for (int i = 0; i < 64; i++)
        {
            Beats.Add(new List<AudioClip>()); // Add 64 sublists for beats
        }
    }


    private void InitializeM1ButtonArray()
    {

        int buttonIndex = 0;
        for (int row = 1; row <= 4; row++)
        {
            Transform rowPanel = mode1.transform.Find("Row" + row);
            if (rowPanel == null) continue;

            for (int bar = 1; bar <= 4; bar++)
            {
                Transform barPanel = rowPanel.Find("Bar" + bar);
                if (barPanel == null) continue;

                for (int btn = 1; btn <= 4; btn++)
                {
                    Button button = barPanel.GetChild(btn - 1).GetComponent<Button>();

                    m1Button m1ButtonScript = button.GetComponent<m1Button>();  //Vyt�hne m1Button script z tla��tka
                    if (button != null && buttonIndex < m1Buttons.Length)
                    {
                        m1Buttons[buttonIndex] = button;
                        m1ButtonScript.buttonIndex = buttonIndex; // Nastav� buttonIndex v m1Button scriptu aktu�ln�ho tla��tka na aktu�ln� buttonIndex

                        buttonIndex++;
                    }
                }
            }
        }

        if (buttonIndex != m1Buttons.Length)
        {
            Debug.LogWarning("Na�lo se pouze" + buttonIndex + "tla��tek");
        }
    }  


    // Zavolat poka�d� co vytvo��me novou �adu pro M2
    public void InitializeM2Row(GameObject rowGameObject)
    {
        Debug.Log($"Initializing M2 Row for GameObject: {rowGameObject}");
        List<Button> rowButtons = new List<Button>();

        Transform beatPanel = rowGameObject.transform.Find("Beat Panel");

        for (int sec = 1; sec <= 4; sec++)
        {
            Transform sectionPanel = beatPanel.Find("Section" + sec);
            if (sectionPanel == null) continue;

            for (int bar = 1; bar <= 4; bar++)
            {
                Transform barPanel = sectionPanel.Find("Bar" + bar);
                if (barPanel == null) continue;

                for (int btn = 1; btn <= 4; btn++)
                {
                    Button button = barPanel.GetChild(btn - 1).GetComponent<Button>();
                    if (button != null)
                    {
                        rowButtons.Add(button); // P�id� tla��tka do listu
                    }
                }
            }
        }

        if (rowButtons.Count != 64)
        {
            Debug.LogWarning("Na�lo se pouze " + rowButtons.Count + " tla��tek.");
        }
        else
        {
            mode2Rows.Add(rowButtons); // P�id� list tla��tek do listu M2 �ad
        }
    }

    public void LogBeatsListContents()
    {
        Debug.Log("____________________");
        for (int i = 0; i < Beats.Count; i++)
        {
            if (Beats[i].Count != 0)
            {
               
                string beatInfo = $"Beat {i + 1} has {Beats[i].Count} clips.";
                Debug.Log(beatInfo);
            }
            
        }
    }

}
