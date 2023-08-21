using UnityEngine;
using UnityEngine.Audio;

public class ActiveMusicButton : MonoBehaviour
{
    private const string MusicVolumeText = "MusicVolume";
    private const float Volume = 0f;

    [SerializeField] private AudioMixerGroup _mixerGroup;

    private void OnEnable()
    {
        _mixerGroup.audioMixer.SetFloat(MusicVolumeText, Volume);
    }
}
