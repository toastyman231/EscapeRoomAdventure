using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Material offMat;

    [SerializeField] private Material onMat;

    [SerializeField] private UnityEvent onPressEvent;

    [SerializeField] private MeshRenderer _renderer;

    private bool _pressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MouseOver()
    {
        // TODO: Add mouse over
        InteractionUIController.ShowInteractionUi("Press Button");
    }

    public void Interact()
    {
        if (!_pressed)
        {
            _pressed = true;
            _renderer.material = onMat;
            _renderer.transform.localPosition =
                new Vector3(_renderer.transform.localPosition.x, 0.009f, _renderer.transform.localPosition.z);
            onPressEvent?.Invoke();
        }
        else
        {
            _pressed = false;
            _renderer.material = offMat;
            _renderer.transform.localPosition =
                new Vector3(_renderer.transform.localPosition.x, 0.043f, _renderer.transform.localPosition.z);
        }
    }
}
