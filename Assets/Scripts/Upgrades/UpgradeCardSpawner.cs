using UnityEngine;

public class UpgradeCardSpawner : UpgradeCard
{
    [SerializeField] private SpawnerCones _spawner;

    public override void Upgrade()
    {
        if (CashCounter.CountDollars >= InternalPrice)
        {
            Purchase.Play();
            CashCounter.SpendDollars(InternalPrice);
            InternalPrice += SurplusFactor;
            _spawner.IncreaseCountWaves();
            PriceText.text = InternalPrice.ToString();
        }
        else
        {
            NotEnough.Play();
        }
    }
}
