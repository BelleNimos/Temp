using System;
using UnityEngine;

[RequireComponent(typeof(CarMovement), typeof(CarSound))]
public class Car : MonoBehaviour
{
    private const float DelayStartMove = 2f;
    private const string StartMoveText = "StartMove";

    [SerializeField] private Seat _seat;
    [SerializeField] private Lane _lane;
    [SerializeField] private CarFirstPosition _firstPosition;
    [SerializeField] private CustomerLastTarget _customerLastTarget;

    private CarSound _carSound;
    private CarMovement _movement;
    private Customer _customer;

    public bool IsFree { get; private set; } = true;
    public bool IsFinish { get; private set; } = false;

    public Action onReadyAddedCustomer;

    private void Start()
    {
        _movement = GetComponent<CarMovement>();
        _carSound = GetComponent<CarSound>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<FinishLane>(out FinishLane finish))
            IsFinish = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent<CarFirstPosition>(out CarFirstPosition carFirstPosition))
        {
            if (IsFinish == true)
            {
                _customer.SetTarget(_customerLastTarget.transform);

                _customer = null;
                IsFree = true;
                
                IsFinish = false;
                _carSound.Stop();
            }

            if (IsFree == true)
            {
                _movement.StopMove();

                if (IsFinish == false)
                {
                    onReadyAddedCustomer?.Invoke();
                }
            }
            else
            {
                _customer.transform.position = _seat.transform.position;
                _customer.transform.rotation = _seat.transform.rotation;

                if (_lane.IsReady == true && IsFinish == false)
                {
                    _movement.Invoke(StartMoveText, DelayStartMove);
                }
            }

            if (_movement.IsReady == false)
            {
                transform.position = _firstPosition.transform.position;
                transform.rotation = _firstPosition.transform.rotation;

                if (_customer != null)
                    _carSound.PlayIdle();
            }
        }
    }

    public void AddCustomer(Customer customer)
    {
        _customer = customer;
        _customer.SitDown();
        IsFree = false;
    }
}
