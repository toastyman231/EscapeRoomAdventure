using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    [ShowOnly] private float _totalWeight;

    [SerializeField] private ScalePlate[] plates;

    [SerializeField] private float weightScale;

    [SerializeField] private float yMin;

    [SerializeField] private float yMax;

    public float YMax { get => yMax; private set => yMax = value; }
    public float WeightScale { get => weightScale; private set => weightScale = value; }
    public float YMin { get => yMin; private set => yMin = value; }

    public void MoveOppositeScale(ScalePlate sender, float difference)
    {
        int senderID = Array.IndexOf(plates, sender);

        int oppositeScaleID = (senderID == 0) ? 1 : 0;

        plates[oppositeScaleID].MovePlate(-difference);
    }
}
