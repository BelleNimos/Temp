using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator), typeof(BoxCollider))]
public class ConeUpgrades : MonoBehaviour
{
    private const string Close = "Close";

    [SerializeField] private AdvertisingOperator _advertisingOperator;
    [SerializeField] private AudioSource _departure;
    [SerializeField] private SpawnerCones _spawner;
    [SerializeField] private Cone _conePrefab;
    [SerializeField] private CashCounter _cashCounter;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private int _price;

    private Animator _animator;
    private BoxCollider _boxCollider;

    public int Price => _price;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider>();
        _priceText.text = _price.ToString();
    }

    public void Unlock()
    {
        if (_cashCounter.CountDollars >= _price)
        {
            _advertisingOperator.ShowInterstitial();
            _cashCounter.SpendDollars(_price);
            _spawner.ChangeConePrefab(_conePrefab);
            StartAnimationClose();
            _boxCollider.enabled = false;
        }
    }

    private void StartAnimationClose()
    {
        _animator.SetTrigger(Close);
        _departure.Play();
    }
}
