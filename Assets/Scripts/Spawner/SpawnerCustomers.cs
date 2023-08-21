using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCustomers : MonoBehaviour
{
    [SerializeField] private List<Customer> _customerPrefabs;
    [SerializeField] private Transform _startTransform;
    [SerializeField] private CustomerArea _customerArea;
    [SerializeField] private int _minDelay;
    [SerializeField] private int _maxDelay;

    private bool _isSpawnCustomers = true;

    private void Start()
    {
        StartCoroutine(IntantiateCustomers());
    }

    private void InstantiateCustomer()
    {
        int index = Random.Range(0, _customerPrefabs.Count);

        Customer customer = Instantiate(_customerPrefabs[index], _startTransform.position, Quaternion.identity);

        _customerArea.SetTargetCustomer(customer);
    }

    private IEnumerator IntantiateCustomers()
    {
        while (_isSpawnCustomers == true)
        {
            int delay = Random.Range(_minDelay, _maxDelay);

            InstantiateCustomer();

            yield return new WaitForSeconds(delay);
        }
    }
}
