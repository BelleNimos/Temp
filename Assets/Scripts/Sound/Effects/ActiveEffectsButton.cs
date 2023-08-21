using UnityEngine;
using UnityEngine.Audio;

public class ActiveEffectsButton : MonoBehaviour
{
    private const string EffectsVolumeText = "EffectsVolume";
    private const float Volume = 0f;

    [SerializeField] private AudioMixerGroup _mixerGroup;

    private void OnEnable()
    {
        _mixerGroup.audioMixer.SetFloat(EffectsVolumeText, Volume);
    }
}
