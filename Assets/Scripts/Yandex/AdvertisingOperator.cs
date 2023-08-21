using UnityEngine;
using Agava.YandexGames;

public class AdvertisingOperator : MonoBehaviour
{
    [SerializeField] private PlayAndPauseButtonsSwitcher _playAndPauseButtonsSwitcher;

    public bool IsPlaying { get; private set; }

    public void ShowInterstitial()
    {
        InterstitialAd.Show(StopGame, ContinueGameState);
    }

    public void ShowVideo()
    {
        VideoAd.Show(StopGame, StopGame, ContinueGame);
    }

    public void ShowSticky()
    {
        StickyAd.Show();
    }

    private void StopGame()
    {
        _playAndPauseButtonsSwitcher.Pause();
        IsPlaying = true;
    }

    private void ContinueGame()
    {
        _playAndPauseButtonsSwitcher.Play();
        IsPlaying = false;
    }

    private void ContinueGameState(bool isReady)
    {
        if (isReady == true)
            ContinueGame();
    }
}
