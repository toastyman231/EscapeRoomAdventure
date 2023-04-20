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

    [SerializeField] private bool weightChanged;

    public float YMax { get => yMax; private set => yMax = value; }
    public float WeightScale { get => weightScale; private set => weightScale = value; }
    public float YMin { get => yMin; private set => yMin = value; }

    // Start is called before the first frame update
    void Start()
    {
        weightChanged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!weightChanged) return;

        if (plates[0].TotalWeight == plates[1].TotalWeight)
        {
            Debug.Log("Weight equal!");
            return;
        }

        ScalePlate heavierPlate = (plates[0].TotalWeight > plates[1].TotalWeight) ? plates[0] : plates[1];
        ScalePlate lighterPlate = (plates[0].TotalWeight < plates[1].TotalWeight) ? plates[0] : plates[1];

        float difference = heavierPlate.TotalWeight - lighterPlate.TotalWeight;

        Debug.Log("Difference: " + difference);

        heavierPlate.MovePlate(-difference);
        lighterPlate.MovePlate(difference);

        weightChanged = false;
    }
}
