using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRotatingCylinder : MonoBehaviour
{
    private static DragRotatingCylinder _currentSelection;

    public bool CanRotate { get; set; }

    void Start()
    {
        CanRotate = true;
    }

    private void OnMouseDown()
    {
        if (_currentSelection == null)
        {
            _currentSelection = this;
            transform.GetChild(0).GetChild(1).gameObject.layer = LayerMask.NameToLayer("Outline");
        }
        else
        {
            GameObject temp = _currentSelection.transform.GetChild(0).gameObject;
            temp.transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Default");

            transform.GetChild(0).SetParent(_currentSelection.transform);
            
            temp.transform.SetParent(transform);

            _currentSelection.transform.GetChild(0).localPosition = Vector3.zero;
            transform.GetChild(0).localPosition = Vector3.zero;

            _currentSelection = null;
        }
    }
}
