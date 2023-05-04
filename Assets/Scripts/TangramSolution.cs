using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tangram Solution", menuName = "Tangram Solution")]
public class TangramSolution : ScriptableObject
{
    public Vector3 BigTriangle1Position;
    public Vector3 BigTriangle2Position;
    public Vector3 MediumTrianglePosition;
    public Vector3 SmallTriangle1Position;
    public Vector3 SmallTriangle2Position;
    public Vector3 SquarePosition;
    public Vector3 RhombusPosition;

    public Vector3 BigTriangle1Rotation;
    public Vector3 BigTriangle2Rotation;
    public Vector3 MediumTriangleRotation;
    public Vector3 SmallTriangle1Rotation;
    public Vector3 SmallTriangle2Rotation;
    public Vector3 SquareRotation;
    public Vector3 RhombusRotation;

    public bool BigTangrams;
}
