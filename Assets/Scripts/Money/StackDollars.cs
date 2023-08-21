using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDollars : MonoBehaviour
{
    [SerializeField] private List<Dollar> _dollars;

    private MoneyTarget _moneyTarget;
    private CashCounter _cashCounter;

    public bool IsReady { get; private set; }

    private void Start()
    {
        IsReady = true;
    }

    public void SetMoneyPoint(MoneyTarget moneyPoint)
    {
        _moneyTarget = moneyPoint;
    }

    public void SetCashCounter(CashCounter cashCounter)
    {
        _cashCounter = cashCounter;
    }

    public void StartMove()
    {
        IsReady = false;
        StartCoroutine(StartMovement());
    }

    private IEnumerator DestroyThis()
    {
        while (_dollars.Count > 0)
        {
            if (_dollars[^1] == null)
                _dollars.RemoveAt(_dollars.Count - 1);

            yield return new WaitForSeconds(0.1f);
        }

        if (_dollars.Count == 0)
            Destroy(gameObject);
    }

    private IEnumerator StartMovement()
    {
        for (int i = _dollars.Count - 1; i >= 0; i--)
        {
            _dollars[i].SetMoneyPoint(_moneyTarget);
            _dollars[i].SetCashCounter(_cashCounter);
            _dollars[i].StartMove();

            yield return new WaitForSeconds(0.04f);
        }

        StartCoroutine(DestroyThis());
    }
}
