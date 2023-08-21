using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CustomerMovement), typeof(CustomerAnimator))]
public class Customer : MonoBehaviour
{
    private CustomerMovement _movement;
    private CustomerAnimator _animator;
    private Transform _currentTarget;
    private Exit _exitTarget;
    private StackDollars _stackDollars;
    private MoneyTarget _moneyTarget;
    private CashCounter _cashCounter;
    private int _moneyToWithdraw;
    private float _maxWaitingSeconds;
    private bool _isCalculateTimer;

    public bool IsReadyExit { get; private set; } = false;

    public Action OnReadyExit;
    public Action<Customer> OnTriggerTarget;

    private void Awake()
    {
        _movement = GetComponent<CustomerMovement>();
        _animator = GetComponent<CustomerAnimator>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<CustomerLastTarget>(out CustomerLastTarget customerLastTarget))
        {
            if (_currentTarget == customerLastTarget.transform)
            {
                InstantiateStackDollars();
                GoToExit();
            }
        }

        if (collision.TryGetComponent<Exit>(out Exit exit))
            if (IsReadyExit == true)
                Destroy(gameObject);

        if (collision.TryGetComponent<CustomerFirstTarget>(out CustomerFirstTarget customerFirstTarget))
            if (_currentTarget == customerFirstTarget.transform)
                OnTriggerTarget?.Invoke(this);

        if (collision.TryGetComponent<TargetWZ>(out TargetWZ target))
            if (_currentTarget == target.transform)
                OnTriggerTarget?.Invoke(this);
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.TryGetComponent<Seat>(out Seat seat))
        {
            if (_currentTarget == null)
            {
                transform.position = seat.transform.position;
                transform.rotation = seat.transform.rotation;
            }
        }

        if (collision.TryGetComponent<CustomerFirstTarget>(out CustomerFirstTarget customerFirstTarget))
            if (_currentTarget == customerFirstTarget.transform)
                StopMove();

        if (collision.TryGetComponent<TargetWZ>(out TargetWZ targetWZ))
            if (_currentTarget == targetWZ.transform)
                StopMove();
    }

    public void SetTarget(Transform target)
    {
        _isCalculateTimer = false;

        _currentTarget = target;
        _movement.SetTarget(_currentTarget);

        SetMinDistance();
        SetWorkTimer();
    }

    public void SitDown()
    {
        _animator.SitDown();

        _currentTarget = null;
        _movement.SetTarget(_currentTarget);

        SetWorkTimer();
    }

    public void SetMoneyToWithdraw(int value)
    {
        _moneyToWithdraw = value;
    }

    public void SetMaxWaitingSeconds(float value)
    {
        _maxWaitingSeconds = value;
    }

    public void SetStackDollars(StackDollars stackDollars)
    {
        _stackDollars = stackDollars;
    }

    public void SetCashCounter(CashCounter cashCounter)
    {
        _cashCounter = cashCounter;
    }

    public void SetMoneyTarget(MoneyTarget moneyPoint)
    {
        _moneyTarget = moneyPoint;
    }

    public void SetExitTarget(Exit exit)
    {
        _exitTarget = exit;
    }

    private void SetMinDistance()
    {
        if (_currentTarget.TryGetComponent<Player>(out Player player))
        {
            float distance = 3.5f;
            _movement.SetMinDistance(distance);
        }
        else
        {
            float distance = 0.1f;
            _movement.SetMinDistance(distance);
        }
    }

    private void SetWorkTimer()
    {
        if (_currentTarget != null)
        {
            if (_currentTarget.TryGetComponent<Player>(out Player player) || _currentTarget.TryGetComponent<CustomerFirstTarget>(out CustomerFirstTarget customerFirstTarget))
            {
                _isCalculateTimer = true;
                StartCoroutine(StartTimer());
            }
            else
            {
                _isCalculateTimer = false;
            }
        }
        else
        {
            _isCalculateTimer = false;
        }
    }

    private void StopMove()
    {
        transform.position = _currentTarget.position;
        _movement.RemoveTarget();
        _movement.DisableAgent();
    }

    private void TakeMoney()
    {
        if (_cashCounter.CountDollars >= _moneyToWithdraw)
            _cashCounter.SpendDollars(_moneyToWithdraw);

        GoToExit();
    }

    private void GoToExit()
    {
        OnReadyExit?.Invoke();
        SetTarget(_exitTarget.transform);
        IsReadyExit = true;
    }

    private void InstantiateStackDollars()
    {
        float rotationX = 0f;
        float rotationY = 20f;
        float rotationZ = 0f;
        StackDollars stackDollars = Instantiate(_stackDollars, transform.position, Quaternion.Euler(rotationX, rotationY, rotationZ));
        stackDollars.SetMoneyPoint(_moneyTarget);
        stackDollars.SetCashCounter(_cashCounter);
    }

    private IEnumerator StartTimer()
    {
        float waitingSeconds = 0f;

        while (_isCalculateTimer == true)
        {
            waitingSeconds += Time.deltaTime;

            if (waitingSeconds >= _maxWaitingSeconds)
            {
                _isCalculateTimer = false;
                TakeMoney();
            }

            yield return new WaitForSeconds(0);
        }
    }
}
