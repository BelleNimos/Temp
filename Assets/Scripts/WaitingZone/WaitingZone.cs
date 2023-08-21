using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitingZone : MonoBehaviour
{
    private const int MaxCustomersCount = 4;

    [SerializeField] private List<TargetWZ> _targetsWZList;
    [SerializeField] private TMP_Text _countCustomersText;
    [SerializeField] private TMP_Text _maxCountCustomersText;
    [SerializeField] private Car _car;

    private Queue<Customer> _customers;
    private Queue<TargetWZ> _customerTargetsWZQueue;

    public int MaxCountCustomers => MaxCustomersCount;
    public int CurrentCustomersCount => _customerTargetsWZQueue.Count;

    private void Awake()
    {
        _customers = new Queue<Customer>();
        _customerTargetsWZQueue = new Queue<TargetWZ>();

        _maxCountCustomersText.text = MaxCustomersCount.ToString();
        _countCustomersText.text = CurrentCustomersCount.ToString();
    }

    private void OnEnable()
    {
        _car.onReadyAddedCustomer += GiveAwayCustomer;
    }

    private void OnDisable()
    {
        _car.onReadyAddedCustomer -= GiveAwayCustomer;
    }

    public void SetTargetCustomer(Customer customer)
    {
        customer.OnTriggerTarget += AddCustomer;

        int index = _targetsWZList.Count - 1;
        customer.SetTarget(_targetsWZList[index].transform);
        _customerTargetsWZQueue.Enqueue(_targetsWZList[index]);
        _targetsWZList.RemoveAt(index);
    }

    private void AddCustomer(Customer customer)
    {
        customer.OnTriggerTarget -= AddCustomer;

        _customers.Enqueue(customer);

        _countCustomersText.text = CurrentCustomersCount.ToString();
    }

    private void GiveAwayCustomer()
    {
        if (_customers.Count > 0 && _car.IsFree == true)
        {
            Customer customer = _customers.Dequeue();

            _targetsWZList.Add(_customerTargetsWZQueue.Dequeue());

            _car.AddCustomer(customer);

            _countCustomersText.text = CurrentCustomersCount.ToString();
        }
    }
}
