    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using System.Collections.Generic;

    public class m1Button : MonoBehaviour, IDropHandler
    {
        public int buttonIndex;

        public List<Image> quadrantImages;

        private const int MaxSamplesPerBeat = 4; // A constant to define max samples per beat


        void Start()    // Nastaví barvu kvadrantù na transparentní
        {
            ClearQuadrantColors();

        // Add onClick listener to the Button component to log AudioClip names
        GetComponent<Button>().onClick.AddListener(LogAudioClipNames);
    }


    private void LogAudioClipNames()
    {
        List<SampleData> beatSamples = GameManager.Instance.Beats[buttonIndex];
        if (beatSamples.Count > 0)
        {
            Debug.Log($"Beat {buttonIndex} contains:");
            foreach (var sample in beatSamples)
            {
                Debug.Log(sample.audioClip.name);
            }
        }
        else
        {
            Debug.Log($"Beat {buttonIndex + 1} contains no samples.");
        }
    }








    public void OnDrop(PointerEventData eventData)
        {
            GameObject droppedObject = eventData.pointerDrag;

            if (droppedObject == null)
            {
                Debug.LogWarning($"Dropped object was null on button with index {buttonIndex}.");
                return;
            }

            SoundData soundData = droppedObject.GetComponent<SoundData>();
            if (soundData == null)
            {
                Debug.LogError($"SoundData component not found on dropped object for button with index {buttonIndex}.");
                return;
            }

            AudioClip droppedClip = soundData.soundClip;
            Image droppedImage = droppedObject.GetComponent<Image>();

            if (droppedClip == null || droppedImage == null)
            {
                Debug.LogError($"AudioClip or Image component not found on dropped object for button with index {buttonIndex}.");
                return;
            }

            Color droppedColor = droppedImage.color;

            if (GameManager.Instance.Beats[buttonIndex].Count >= MaxSamplesPerBeat)
            {
                Debug.LogWarning($"Max samples per beat reached on button with index {buttonIndex}.");
                return;
            }

            SampleData newSample = new SampleData(droppedClip, droppedColor);
            if (!GameManager.Instance.Beats[buttonIndex].Exists(sd => sd.audioClip == droppedClip))
            {
                GameManager.Instance.Beats[buttonIndex].Add(newSample);
                UpdateQuadrantAppearance(droppedColor);
                GameManager.Instance.LogBeatsListContents(buttonIndex);
            }
            else
            {
                Debug.Log($"This Sample is already assigned to this beat for button with index {buttonIndex}.");
            }
        }



        private void UpdateQuadrantAppearance(Color color)
        {
            ClearQuadrantColors();



            var beatsCount = GameManager.Instance.Beats[buttonIndex].Count;


            if (beatsCount == 1)    // Pokud je Sample 1, zmìní barvy všech kvadrantù na barvu tohoto samplu
            {
                foreach (var quadrantImage in quadrantImages)
                {
                    quadrantImage.color = color;
                }
            }
            else if (beatsCount == 2)   // Pokud jsou 2, zbarví 2 kvadranty podle 1. samplu, a druhé 2 podle druhého samplu
            {
                quadrantImages[0].color = GameManager.Instance.Beats[buttonIndex][0].color;
                quadrantImages[1].color = GameManager.Instance.Beats[buttonIndex][0].color;

                quadrantImages[2].color = GameManager.Instance.Beats[buttonIndex][1].color;
                quadrantImages[3].color = GameManager.Instance.Beats[buttonIndex][1].color;
            }
            else if (beatsCount == 3)       // Pokud jsou 3, zbarví první 3 kvadranty podle tìch 3 samplù a 4. kvadrantu dá barvu 3. samplu
            {
                for (int j = 0; j < beatsCount; j++)
                {
                    quadrantImages[j].color = GameManager.Instance.Beats[buttonIndex][j].color;
                }

                quadrantImages[3].color = GameManager.Instance.Beats[buttonIndex][2].color;
            }
            else if (beatsCount > 3)    // Pro více než 3 samply, zbarví každý kvadrant podle každého samplu
            {
                for (int j = 0; j < beatsCount && j < quadrantImages.Count; j++)
                {
                    quadrantImages[j].color = GameManager.Instance.Beats[buttonIndex][j].color;
                }
            }
        }
















        public void ClearQuadrantColors()
        {
            foreach (var image in quadrantImages)
            {
                image.color = Color.clear;
            }
        }

    }
