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

    void Start()
    {
        // Najde GameManager v instanci scény
        gameManager = GameManager.Instance;

        fourthNoteButton.onClick.AddListener(() => SetTimeSignature(1));
        eighthNoteButton.onClick.AddListener(() => SetTimeSignature(2));
        sixteenthNoteButton.onClick.AddListener(() => SetTimeSignature(4));
    }

    private void SetTimeSignature(int timeSignature)
    {
        gameManager.SetCurrentTimeSignature(timeSignature);
    }
}
