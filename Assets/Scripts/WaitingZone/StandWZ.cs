using UnityEngine;

[RequireComponent(typeof(Waypoint_Indicator))]
public class StandWZ : MonoBehaviour
{
    [SerializeField] private Scale _scale;
    [SerializeField] private WaitingZone _waitingZone;
    [SerializeField] private Player _player;

    private Waypoint_Indicator _waypointIndicator;

    private bool _isReady = false;

    private void Awake()
    {
        _waypointIndicator = GetComponent<Waypoint_Indicator>();
    }

    private void Start()
    {
        if (_player.CurrentCustomersCount <= 0)
            DisableIndicator();
        else
            EnableIndicator();
    }

    private void OnEnable()
    {
        _player.OnEmptyList += EnableIndicator;
        _player.OnRemovedCustomer += DisableIndicator;
        _scale.OnSliderFilled += OnReadyAddCustomer;
    }

    private void OnDisable()
    {
        _player.OnEmptyList -= EnableIndicator;
        _player.OnRemovedCustomer -= DisableIndicator;
        _scale.OnSliderFilled -= OnReadyAddCustomer;
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
            if (_isReady == true)
                AddCustomer(player);
            else
                if (player.CurrentCustomersCount > 0)
                    if (_waitingZone.CurrentCustomersCount < _waitingZone.MaxCountCustomers)
                        EnableSlider();
        }
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
        DisableIndicator();
    }

    private void DisableSlider()
    {
        _scale.gameObject.SetActive(false);
        _isReady = false;
    }

    private void AddCustomer(Player player)
    {
        DisableSlider();
        player.GiveAwayCustomer(_waitingZone);
    }

    private void OnReadyAddCustomer()
    {
        _isReady = true;
    }
}
