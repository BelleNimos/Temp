using UnityEngine;
using Agava.WebUtility;

public class WebFocus : MonoBehaviour
{
    [SerializeField] private AdvertisingOperator _advertisingOperator;

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
    }

    private void OnInBackgroundChange(bool inBackground)
    {
        if (_advertisingOperator != null)
        {
            if (_advertisingOperator.IsPlaying == true)
            {
                AudioListener.pause = true;
                AudioListener.volume = 0f;
            }
            else
            {
                AudioListener.pause = inBackground;
                AudioListener.volume = inBackground ? 0f : 1f;
            }
        }
        else
        {
            AudioListener.pause = inBackground;
            AudioListener.volume = inBackground ? 0f : 1f;
        }
    }
}
