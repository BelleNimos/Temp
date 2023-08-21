using UnityEngine;
using Agava.YandexGames;

public class LeaderboardSwitcher : MonoBehaviour
{
    [SerializeField] private YandexLeaderboard _yandexLeaderboard;

    private bool _isActive = false;

    public void Show()
    {
        if (_isActive == false)
            Open();
        else
            Close();
    }

    public void Open()
    {
        //bool test = false;

        //if (test == true)
        //{
        //    _yandexLeaderboard.FormListOfTopPlayers();
        //    _yandexLeaderboard.Expand();
        //    _isActive = true;

        //    return;
        //}

        PlayerAccount.Authorize();

        if (PlayerAccount.IsAuthorized == true)
            PlayerAccount.RequestPersonalProfileDataPermission(ShowLeaderboard);
    }

    public void Close()
    {
        _yandexLeaderboard.Minimize();
        _isActive = false;
    }

    private void ShowLeaderboard()
    {
        _yandexLeaderboard.FormListOfTopPlayers();
        _yandexLeaderboard.Expand();
        _isActive = true;
    }
}
