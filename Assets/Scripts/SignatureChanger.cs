using UnityEngine;
using UnityEngine.UI;

[System.Serializable] // This makes the Row class able to be shown in the Inspector
public class Row
{
    public GameObject[] beats; // This array will show in the Inspector
}

public class SignatureChanger : MonoBehaviour
{

    [SerializeField] private Button fourFourButton;
    [SerializeField] private Button eightFourButton;
    [SerializeField] private Button sixteenFourButton;

    public Row[] m1BeatsRows = new Row[4]; // This will create 4 rows for m1Beats
    public Row[] m2BeatsRows = new Row[6]; // This will create 6 rows for m2Beats

    void Start()
    {
        fourFourButton.onClick.AddListener(() => ChangeSignature(4));
        eightFourButton.onClick.AddListener(() => ChangeSignature(2));
        sixteenFourButton.onClick.AddListener(() => ChangeSignature(1));

        for (int i = 0; i < m1BeatsRows.Length; i++)
            m1BeatsRows[i] = new Row { beats = new GameObject[16] };
        for (int i = 0; i < m2BeatsRows.Length; i++)
            m2BeatsRows[i] = new Row { beats = new GameObject[64] };
    }

    private void ChangeSignature(int division)
    {
        DeactivateAllBeats(m1BeatsRows);
        DeactivateAllBeats(m2BeatsRows);

        ActivateBeats(m1BeatsRows, division);
        ActivateBeats(m2BeatsRows, division);
    }

    private void DeactivateAllBeats(Row[] beatsRows)
    {
        foreach (Row row in beatsRows)
        {
            foreach (GameObject beat in row.beats)
            {
                beat.SetActive(false);
            }
        }
    }

    private void ActivateBeats(Row[] beatsRows, int division)
    {
        foreach (Row row in beatsRows)
        {
            for (int beat = 0; beat < row.beats.Length; beat += division)
            {
                if (row.beats[beat] != null) // Check to avoid null reference
                    row.beats[beat].SetActive(true);
            }
        }
    }
}
