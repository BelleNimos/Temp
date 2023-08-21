using UnityEngine;
using UnityEngine.Audio;

public class InactiveMusicButton : MonoBehaviour
{
    private const string MusicVolumeText = "MusicVolume";
    private const float Volume = -80f;

    [SerializeField] private AudioMixerGroup _mixerGroup;

    private void OnEnable()
    {
        _mixerGroup.audioMixer.SetFloat(MusicVolumeText, Volume);
    }
}
