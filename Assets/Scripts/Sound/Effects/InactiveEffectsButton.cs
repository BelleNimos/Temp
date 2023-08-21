using UnityEngine;
using UnityEngine.Audio;

public class InactiveEffectsButton : MonoBehaviour
{
    private const string EffectsVolumeText = "EffectsVolume";
    private const float Volume = -80f;

    [SerializeField] private AudioMixerGroup _mixerGroup;

    private void OnEnable()
    {
        _mixerGroup.audioMixer.SetFloat(EffectsVolumeText, Volume);
    }
}
