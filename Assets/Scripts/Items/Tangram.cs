using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

// Object drag script from https://answers.unity.com/questions/12322/drag-gameobject-with-mouse.html
public class Tangram : MonoBehaviour
{
    private static TangramSolution _currentSolution;
    private static TangramPuzzleInteractable _tangramParent;
    private static Tangram _setupTangram;

    private static int[] _usedIndexes;
    private static int _solves = 0;

    public static Tangram bigTriangle1;
    public static Tangram bigTriangle2;
    public static Tangram mediumTriangle;
    public static Tangram smallTriangle1;
    public static Tangram smallTriangle2;
    public static Tangram square;
    public static Tangram rhombus;

    [SerializeField] private float rotationAmount;

    [SerializeField] private TangramPiece pieceType;

    [SerializeField] private float tolerance;

    [SerializeField] private float maxAngle;

    [SerializeField] private bool resetAngle;

    private Rigidbody _rb;

    private Vector3 _screenPoint;
    private Vector3 _offset;

    private float _rotZ = 0f;

    private void Start()
    {
        if (_setupTangram == null)
        {
            _setupTangram = this;
            _tangramParent = GetComponentInParent<TangramPuzzleInteractable>();

            _usedIndexes = new int[_tangramParent.requiredSolves];

            for (int i = 0; i < _usedIndexes.Length; i++) _usedIndexes[i] = -1;

            _currentSolution = GetRandomSolution(out var chosenIndex);
            _tangramParent.SetNewPuzzleImage(chosenIndex);
            _usedIndexes[0] = chosenIndex;
        }

        _rb = GetComponent<Rigidbody>();

        switch (pieceType)
        {
            case TangramPiece.BigTriangle1:
                bigTriangle1 = this;
                break;
            case TangramPiece.BigTriangle2:
                bigTriangle2 = this;
                break;
            case TangramPiece.MediumTriangle:
                mediumTriangle = this;
                break;
            case TangramPiece.SmallTriangle1:
                smallTriangle1 = this;
                break;
            case TangramPiece.SmallTriangle2:
                smallTriangle2 = this;
                break;
            case TangramPiece.Square:
                square = this;
                break;
            case TangramPiece.Rhombus:
                rhombus = this;
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 5f))
            {
                //Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red, 10f);
                if (hit.collider.gameObject == gameObject)
                {
                    _rotZ += rotationAmount;
                    if (_rotZ > maxAngle) _rotZ -= maxAngle;
                    _rb.rotation = Quaternion.Euler(-90f, 0f, _rotZ);
                    if (resetAngle && _rotZ != 0) _rotZ = -rotationAmount;
                } else if (transform.childCount > 0)
                {
                    if (hit.collider.gameObject == transform.GetChild(0).gameObject)
                    {
                        _rotZ += rotationAmount;
                        if (_rotZ > maxAngle) _rotZ -= maxAngle;
                        _rb.rotation = Quaternion.Euler(-90f, 0f, _rotZ);
                        if (resetAngle && _rotZ != 0) _rotZ = -rotationAmount;
                    }
                }
            }
        }
    }

    private bool CheckSolution()
    {
        if (_currentSolution == null) return false;

        if (Vector3.Distance(bigTriangle1.transform.position, _currentSolution.BigTriangle1Position) >
            tolerance) return false;
        if (Vector3.Distance(bigTriangle2.transform.position, _currentSolution.BigTriangle2Position) >
            tolerance) return false;
        if (Vector3.Distance(mediumTriangle.transform.position, _currentSolution.MediumTrianglePosition) >
            tolerance) return false;
        if (Vector3.Distance(smallTriangle1.transform.position, _currentSolution.SmallTriangle1Position) >
            tolerance) return false;
        if (Vector3.Distance(smallTriangle2.transform.position, _currentSolution.SmallTriangle2Position) >
            tolerance) return false;
        if (Vector3.Distance(square.transform.position, _currentSolution.SquarePosition) >
            tolerance) return false;
        if (Vector3.Distance(rhombus.transform.position, _currentSolution.RhombusPosition) >
            tolerance) return false;

        if (Vector3.Distance(bigTriangle1.transform.localEulerAngles, _currentSolution.BigTriangle1Rotation) >
            tolerance) return false;
        if (Vector3.Distance(bigTriangle2.transform.localEulerAngles, _currentSolution.BigTriangle2Rotation) >
            tolerance) return false;
        if (Vector3.Distance(mediumTriangle.transform.localEulerAngles, _currentSolution.MediumTriangleRotation) >
            tolerance) return false;
        if (Vector3.Distance(smallTriangle1.transform.localEulerAngles, _currentSolution.SmallTriangle1Rotation) >
            tolerance) return false;
        if (Vector3.Distance(smallTriangle2.transform.localEulerAngles, _currentSolution.SmallTriangle2Rotation) >
            tolerance) return false;
        if (Vector3.Distance(square.transform.localEulerAngles, _currentSolution.SquareRotation) >
            tolerance) return false;
        if (Vector3.Distance(rhombus.transform.localEulerAngles, _currentSolution.RhombusRotation) >
            tolerance) return false;

        return true;
    }

    private void OnPuzzleSolved()
    {
        _solves++;

        if (_solves < _tangramParent.requiredSolves)
        {
            ResetPuzzle(_tangramParent.defaultPositions);
            _currentSolution = GetRandomSolution(out var chosenIndex);
            _tangramParent.SetNewPuzzleImage(chosenIndex);
        }
        else
        {
            _tangramParent.OnAllSolvesCompleted();
        }
    }

    private static TangramSolution GetRandomSolution(out int chosenIndex, int index = -1)
    {
        TangramSolution[] solutions = Resources.LoadAll<TangramSolution>("Tangram Solutions/");
        int[] possibleIndexes = new int[solutions.Length];

        for (int i = 0; i < solutions.Length; i++) possibleIndexes[i] = i;

        IEnumerable<int> validIndexes = possibleIndexes.Where((num) => !_usedIndexes.Contains(num));

        index = (index < 0) ? validIndexes.ElementAt(Random.Range(0, validIndexes.Count())) : index;

        chosenIndex = index;
        _usedIndexes[_solves] = chosenIndex;
        TangramSolution chosenSolution = solutions[index];
        _tangramParent.ResizeTangrams(chosenSolution.BigTangrams);

        Resources.UnloadUnusedAssets();
        return chosenSolution;
    }

    private void OnMouseDown()
    {
        _screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        _offset = gameObject.transform.position -
                  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                      _screenPoint.z));
    }

    private void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + _offset;
        _rb.position = curPosition;
    }

    private void OnMouseUp()
    {
        if (CheckSolution()) OnPuzzleSolved();
    }

    private static void ResetPuzzle(TangramSolution defaultPositions)
    {
        bigTriangle1.transform.position = defaultPositions.BigTriangle1Position;
        bigTriangle1.transform.localEulerAngles = defaultPositions.BigTriangle1Rotation;

        bigTriangle2.transform.position = defaultPositions.BigTriangle2Position;
        bigTriangle2.transform.localEulerAngles = defaultPositions.BigTriangle2Rotation;

        mediumTriangle.transform.position = defaultPositions.MediumTrianglePosition;
        mediumTriangle.transform.localEulerAngles = defaultPositions.MediumTriangleRotation;

        smallTriangle1.transform.position = defaultPositions.SmallTriangle1Position;
        smallTriangle1.transform.localEulerAngles = defaultPositions.SmallTriangle1Rotation;

        smallTriangle2.transform.position = defaultPositions.SmallTriangle2Position;
        smallTriangle2.transform.localEulerAngles = defaultPositions.SmallTriangle2Rotation;

        square.transform.position = defaultPositions.SquarePosition;
        square.transform.localEulerAngles = defaultPositions.SquareRotation;

        rhombus.transform.position = defaultPositions.RhombusPosition;
        rhombus.transform.localEulerAngles = defaultPositions.RhombusRotation;
    }

    private enum TangramPiece
    {
        BigTriangle1,
        BigTriangle2,
        MediumTriangle,
        SmallTriangle1,
        SmallTriangle2,
        Square,
        Rhombus
    }
}
