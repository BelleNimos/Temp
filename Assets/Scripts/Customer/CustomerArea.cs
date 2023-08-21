using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Waypoint_Indicator))]
public class CustomerArea : MonoBehaviour
{
    [SerializeField] private List<CustomerFirstTarget> _customerFirstTargetsList;
    [SerializeField] private Scale _scale;
    [SerializeField] private StackDollars _stackDollarsPrefab;
    [SerializeField] private MoneyTarget _moneyTarget;
    [SerializeField] private CashCounter _cashCounter;
    [SerializeField] private Exit _exitTarget;
    [SerializeField] private int _moneyToWithdraw;
    [SerializeField] private float _maxWaitingSeconds;

    private Waypoint_Indicator _waypointIndicator;
    private Queue<Customer> _customers;
    private Queue<CustomerFirstTarget> _customerFirstTargetsQueue;
    private bool _isReady = false;

    private void Awake()
    {
        _waypointIndicator = GetComponent<Waypoint_Indicator>();
        _customers = new Queue<Customer>();
        _customerFirstTargetsQueue = new Queue<CustomerFirstTarget>();

        if (_customers.Count <= 0)
            DisableIndicator();
        else
            EnableIndicator();
    }

    private void OnEnable()
    {
        _scale.OnSliderFilled += OnReadyTransferCustomer;
    }

    private void OnDisable()
    {
        _scale.OnSliderFilled -= OnReadyTransferCustomer;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            if (_isReady == true)
                GiveAwayCustomer(player.AddCustomer);
            else
                if (_customers.Count > 0)
                    EnableSlider();
                else
                    DisableSlider();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
            DisableSlider();
    }

    public void SetTargetCustomer(Customer customer)
    {
        SetStartValues(customer);

        customer.OnTriggerTarget += AddCustomer;
        customer.OnReadyExit += RemoveCustomer;

        int index = _customerFirstTargetsList.Count - 1;
        customer.SetTarget(_customerFirstTargetsList[index].transform);
        _customerFirstTargetsQueue.Enqueue(_customerFirstTargetsList[index]);
        _customerFirstTargetsList.RemoveAt(index);
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
        _waypointIndicator.enableStandardTracking = false;
        _scale.gameObject.SetActive(true);

        DisableIndicator();
    }

    private void DisableSlider()
    {
        _scale.gameObject.SetActive(false);
        _isReady = false;

        if (_customers.Count > 0)
            EnableIndicator();
    }

    private void GiveAwayCustomer(Action<Customer> onGiveAwayCustomer)
    {
        DisableSlider();

        Customer customer = _customers.Dequeue();

        _customerFirstTargetsList.Add(_customerFirstTargetsQueue.Dequeue());

        customer.OnTriggerTarget -= AddCustomer;
        customer.OnReadyExit -= RemoveCustomer;

        onGiveAwayCustomer(customer);

        if (_customers.Count <= 0)
            DisableIndicator();
    }

    private void AddCustomer(Customer customer)
    {
        customer.OnTriggerTarget -= AddCustomer;

        _customers.Enqueue(customer);

        if (_customers.Count > 0)
            EnableIndicator();
    }

    private void RemoveCustomer()
    {
        Customer customer = _customers.Dequeue();

        _customerFirstTargetsList.Add(_customerFirstTargetsQueue.Dequeue());

        customer.OnTriggerTarget -= AddCustomer;
        customer.OnReadyExit -= RemoveCustomer;
    }

    private void SetStartValues(Customer customer)
    {
        customer.SetMoneyToWithdraw(_moneyToWithdraw);
        customer.SetMaxWaitingSeconds(_maxWaitingSeconds);
        customer.SetStackDollars(_stackDollarsPrefab);
        customer.SetCashCounter(_cashCounter);
        customer.SetMoneyTarget(_moneyTarget);
        customer.SetExitTarget(_exitTarget);
    }

    private void OnReadyTransferCustomer()
    {
        _isReady = true;
    }
}
