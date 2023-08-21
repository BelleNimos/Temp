using System.Collections.Generic;
using UnityEngine;

public class SpawnerPrefabs : MonoBehaviour
{
    public static SpawnerPrefabs SpawnerPrefabsInstance = null;

    [SerializeField] private List<Cone> _conePrefabs;

    private void Awake()
    {
        if (SpawnerPrefabsInstance == null)
        {
            SpawnerPrefabsInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GiveAwayPrefab(SpawnerCones spawner, int indexCone)
    {
        foreach (var cone in _conePrefabs)
            if (cone.Index == indexCone)
                spawner.ChangeConePrefab(cone);
    }
}
