using UnityEngine;
using UnityEngine.UI;

public class SignatureChanger : MonoBehaviour
{
    [SerializeField]
    private Button fourthNoteButton; 
    [SerializeField]
    private Button eighthNoteButton;
    [SerializeField]
    private Button sixteenthNoteButton;

    private GameManager gameManager;
    public static AudioPlaybackManager audioPlaybackManager;

    void Start()
    {
        // Najde GameManager v instanci scény
        gameManager = GameManager.Instance;
        audioPlaybackManager = AudioPlaybackManager.Instance;

        fourthNoteButton.onClick.AddListener(() => SetTimeSignature(4));
        eighthNoteButton.onClick.AddListener(() => SetTimeSignature(8));
        sixteenthNoteButton.onClick.AddListener(() => SetTimeSignature(16));
    }

    private void SetTimeSignature(int timeSignature)
    {
        gameManager.SetCurrentTimeSignature(timeSignature);
        audioPlaybackManager.SetCurrentTimeSignature(timeSignature);


    }
}
