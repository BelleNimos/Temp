using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Waypoint_Indicator))]
public class SpawnerCones : MonoBehaviour
{
    private const int ConesInWaveCount = 4;
    private const float DelayGiveAway = 0.05f;

    [SerializeField] private List<Transform> _points;
    [SerializeField] private Scale _scale;
    [SerializeField] private Cone _startConePrefab;
    [SerializeField] private CashCounter _cashCounter;

    private Waypoint_Indicator _waypointIndicator;
    private Stack<Cone> _cones;
    private Cone _conePrefab;
    private int _countWaves = 5;
    private float _timer = 0f;
    private bool _isReady = false;
    
    public int IndexCone => _conePrefab.Index;
    public int CountWaves => _countWaves;

    private void Awake()
    {
        _waypointIndicator = GetComponent<Waypoint_Indicator>();
        _cones = new Stack<Cone>();
        _conePrefab = _startConePrefab;

        if (_cones.Count > 0)
            DisableIndicator();
        else
            EnableIndicator();
    }

    private void OnEnable()
    {
        _scale.OnSliderFilled += SpawnCones;
    }

    private void OnDisable()
    {
        _scale.OnSliderFilled -= SpawnCones;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
            DisableSlider();
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            if (_isReady)
                player.AddCones(GiveAwayCone);

            if (_cones.Count == 0)
                EnableSlider();
        }
    }

    public void SetDefaultValues()
    {
        _conePrefab = _startConePrefab;
    }

    public void SetStartValues(int countWaves, int indexCone)
    {
        _countWaves = countWaves;
        SpawnerPrefabs.SpawnerPrefabsInstance.GiveAwayPrefab(this, indexCone);
    }

    public void IncreaseCountWaves()
    {
        _countWaves++;
    }

    public void ChangeConePrefab(Cone cone)
    {
        _conePrefab = cone;
    }

    private void EnableIndicator()
    {
        _waypointIndicator.enableStandardTracking = true;
    }

    private void DisableIndicator()
    {
        _waypointIndicator.enableStandardTracking = false;
    }

    private void EnableSlider()
    {
        _scale.gameObject.SetActive(true);

        _waypointIndicator.enableStandardTracking = false;
    }

    private void DisableSlider()
    {
        _scale.gameObject.SetActive(false);

        if (_cones.Count > 0)
            DisableIndicator();
        else
            EnableIndicator();
    }

    private void SpawnCones()
    {
        _scale.gameObject.SetActive(false);
        StartCoroutine(InstantiateCones());
    }

    private void GiveAwayCone(Bag bag)
    {
        if (_cones.Count > 0)
        {
            if (_timer >= DelayGiveAway)
            {
                bag.AddCone(_cones.Pop());
                _timer = 0f;
            }
        }

        if (_cones.Count > 0)
            _isReady = true;
        else
            _isReady = false;

        if (_cones.Count > 0)
            DisableIndicator();
        else
            EnableIndicator();
    }

    private IEnumerator InstantiateCones()
    {
        float distanceCoefficient = 0.25f;

        for (int i = 0; i < _countWaves; i++)
        {
            Cone cone;

            float positionX;
            float positionY;
            float positionZ;

            for (int j = 0; j < _points.Count; j++)
            {
                positionX = _points[j].position.x;
                positionY = _points[j].position.y;
                positionZ = _points[j].position.z;

                positionY = (positionY + i) * distanceCoefficient;
                Vector3 position = new Vector3(positionX, positionY, positionZ);
                cone = Instantiate(_conePrefab, position, Quaternion.Euler(0f, 20f, 0f));
                cone.SetCashCounter(_cashCounter);
                _cones.Push(cone);

                yield return new WaitForSeconds(0.1f);
            }
        }

        if (_cones.Count == ConesInWaveCount * _countWaves)
            _isReady = true;
    }
}
