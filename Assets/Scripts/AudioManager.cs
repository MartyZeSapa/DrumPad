    using UnityEngine;

    public class AudioManager : MonoBehaviour
    {
        void Awake()
        {
            AudioConfiguration config = AudioSettings.GetConfiguration();
            config.dspBufferSize = 200; // Lower buffer size can reduce latency
            AudioSettings.Reset(config);

            Debug.Log("Audio configuration set: DSP buffer size = " + config.dspBufferSize);
        }
    }