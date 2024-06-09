using UnityEngine;
using UnityEngine.UI;

public class MetronomeHandler : MonoBehaviour
{
    [SerializeField]
    private Button button1, button2, button3, resetButton;

    [SerializeField]
    private Image button1Image, button2Image, button3Image;


    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip metronomeSound1, metronomeSound2, metronomeSound3;

    //private int currentMetronomeIndex = 0;
    //private int timeSignature = 4;

    private Color defaultColor = new Color32(80, 80, 80, 255);
    private Color clickedColor = new Color32(140, 140, 140, 255);

    private Color highlightColor = new Color32(200, 200, 200, 255); // Flash color (red)

    public bool isReset = false;





    void Start()
    {
        button1.onClick.AddListener(() => SetMetronomeSound(1));
        button2.onClick.AddListener(() => SetMetronomeSound(2));
        button3.onClick.AddListener(() => SetMetronomeSound(3));

        resetButton.onClick.AddListener(ResetMetronome);
    }

    private void SetMetronomeSound(int soundIndex)
    {

        SetActiveMetronome();


        AudioClip selectedSound = soundIndex switch
        {
            1 => metronomeSound1,
            2 => metronomeSound2,
            3 => metronomeSound3,
            _ => null,
        };

        if (audioSource != null && selectedSound != null)
        {
            audioSource.clip = selectedSound;
            Debug.Log(selectedSound);
        }

        UpdateButtonColors(soundIndex);
    }



    private void UpdateButtonColors(int clickedButtonIndex)
    {
        button1Image.color = defaultColor;
        button2Image.color = defaultColor;
        button3Image.color = defaultColor;

        if (clickedButtonIndex == 1)
        {
            button1Image.color = clickedColor;
        }
        else if (clickedButtonIndex == 2)
        {
            button1Image.color = clickedColor;
            button2Image.color = clickedColor;
        }
        else if (clickedButtonIndex == 3)
        {
            button1Image.color = clickedColor;
            button2Image.color = clickedColor;
            button3Image.color = clickedColor;
        }
    }



    public void PlayMetronome()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }


    public void HighlightButton()
    {
        if (button1Image.color == clickedColor)
            button1Image.color = highlightColor;
        if (button2Image.color == clickedColor)
            button2Image.color = highlightColor;
        if (button3Image.color == clickedColor)
            button3Image.color = highlightColor;
    }

    public void UnhighlightButton()
    {
        if (button1Image.color == highlightColor)
            button1Image.color = clickedColor;
        if (button2Image.color == highlightColor)
            button2Image.color = clickedColor;
        if (button3Image.color == highlightColor)
            button3Image.color = clickedColor;
    }




    public void ResetMetronome()
    {
        isReset = true;
        audioSource.clip = null;
        button1Image.color = defaultColor;
        button2Image.color = defaultColor;
        button3Image.color = defaultColor;
    }

    public void SetActiveMetronome()
    {
        isReset = false;
    }

}

