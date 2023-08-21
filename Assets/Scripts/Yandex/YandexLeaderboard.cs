using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;

public class YandexLeaderboard : MonoBehaviour
{
    private const string LeaderboardName = "Leaderboard";
    private const string ExpandText = "Expand";
    private const string MinimizeText = "Minimize";

    [SerializeField] private LeaderboardView _leaderboardView;
    [SerializeField] private AudioSource _expandSound;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void FormListOfTopPlayers()
    {
        List<PlayerInfoLeaderboard> topFivePlayers = new();

        //bool test = false;

        //if (test == true)
        //{
        //    for (int i = 0; i < 5; i++)
        //        topFivePlayers.Add(new PlayerInfoLeaderboard("Name", i));

        //    _leaderboardView.ConstructLeaderboard(topFivePlayers);

        //    return;
        //}

        if (PlayerAccount.IsAuthorized == true)
        {
            Leaderboard.GetEntries(LeaderboardName, (result) =>
            {
                Debug.Log($"My rank = {result.userRank}");

                int resultsAmount = result.entries.Length;

                resultsAmount = Mathf.Clamp(resultsAmount, 1, 5);

                for (int i = 0; i < resultsAmount; i++)
                {
                    string name = result.entries[i].player.publicName;

                    if (string.IsNullOrEmpty(name))
                        name = "Anonymos";

                    int score = result.entries[i].score;

                    topFivePlayers.Add(new PlayerInfoLeaderboard(name, score));
                }

                _leaderboardView.ConstructLeaderboard(topFivePlayers);
            });
        }
    }

    public void AddPlayerToLeaderboard(int score)
    {
        Leaderboard.GetPlayerEntry(LeaderboardName, (result) =>
        {
            if (result.score < score)
                Leaderboard.SetScore(LeaderboardName, score);
        });
    }

    public void Expand()
    {
        _animator.SetTrigger(ExpandText);
        _expandSound.Play();
    }

    public void Minimize()
    {
        _animator.SetTrigger(MinimizeText);
    }
}

public class PlayerInfoLeaderboard
{
    public string Name { get; private set; }
    public int Score { get; private set; }

    public PlayerInfoLeaderboard(string name, int score)
    {
        Name = name;
        Score = score;
    }
}
