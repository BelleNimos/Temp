using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(Wallet))]
public class Player : MonoBehaviour
{
    [SerializeField] private Bag _bag;

    private Queue<Customer> _customers;
    private PlayerMovement _characterMovement;
    private Wallet _wallet;

    public int CurrentCustomersCount => _customers.Count;

    public Action OnEmptyList;
    public Action OnRemovedCustomer;

    private void Awake()
    {
        _customers = new Queue<Customer>();
        _characterMovement = GetComponent<PlayerMovement>();
        _wallet = GetComponent<Wallet>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<TransitionLevel>(out TransitionLevel transitionLevel))
        {
            _characterMovement.SlowDownSpeed();
            transitionLevel.OpenPanel();
        }

        if (collision.TryGetComponent<UpgradesStand>(out UpgradesStand upgradesStand))
        {
            _characterMovement.SlowDownSpeed();
            upgradesStand.EnableUpgradesPanel();
        }

        if (collision.TryGetComponent<ConePoint>(out ConePoint conePoint))
            if (_bag.CurrentConesCount > 0 && conePoint.IsFree == true)
                _bag.GiveAwayCone(conePoint);

        if (collision.TryGetComponent<Cone>(out Cone cone))
            if (_bag.CurrentConesCount < _bag.MaxConesCount)
                if (cone.IsCollision == true)
                    _bag.AddCone(cone);

        if (collision.TryGetComponent<ConeUpgrades>(out ConeUpgrades conesUpgrade))
            if (_wallet.CountDollars >= conesUpgrade.Price)
                conesUpgrade.Unlock();

        if (collision.TryGetComponent<StackDollars>(out StackDollars stackDollars))
            if (stackDollars.IsReady == true)
                stackDollars.StartMove();
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent<UpgradesStand>(out UpgradesStand upgradesStand))
        {
            _characterMovement.ResetSpeed();
            upgradesStand.DisableUpgradesPanel();
        }

        if (collision.TryGetComponent<TransitionLevel>(out TransitionLevel transitionLevel))
        {
            _characterMovement.ResetSpeed();
            transitionLevel.ClosePanel();
        }
    }

    public void AddCones(Action<Bag> onGiveAwayCone)
    {
        if (_bag.CurrentConesCount < _bag.MaxConesCount)
            onGiveAwayCone(_bag);
    }

    public void GiveAwayCones(Utilizer utilizer)
    {
        if (_bag.CurrentConesCount > 0)
            _bag.GiveAwayCone(utilizer);
    }

    public void AddCustomer(Customer customer)
    {
        customer.SetTarget(transform);
        customer.OnReadyExit += RemoveCustomer;
        _customers.Enqueue(customer);
        OnEmptyList?.Invoke();
    }

    public void RemoveCustomer()
    {
        Customer customer = _customers.Dequeue();
        customer.OnReadyExit -= RemoveCustomer;

        if (CurrentCustomersCount <= 0)
            OnRemovedCustomer?.Invoke();
        else
            OnEmptyList?.Invoke();
    }

    public void GiveAwayCustomer(WaitingZone waitingZone)
    {
        if (_customers.Count > 0)
        {
            Customer customer = _customers.Dequeue();
            customer.OnReadyExit -= RemoveCustomer;
            waitingZone.SetTargetCustomer(customer);
        }
    }
}
