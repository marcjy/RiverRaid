using System;
using UnityEngine;

public class FuelCanister : BaseCollectible
{
    [Range(0, 1)]
    public float PercentageFuelRestored = 0.75f;

    public override void CollectInternal(GameObject player)
    {
        if (player.TryGetComponent(out PlayerFuelManager fuelManager))
            fuelManager.AddFuelPercentage(PercentageFuelRestored);

        TriggerShouldBeReleased();
    }
}