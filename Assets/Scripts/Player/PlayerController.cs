using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [SerializeField] private float stepInterval;
    [SerializeField] private AudioClip[] footstepSounds;

    private AudioSource _audioSource;

    private MouseLook _mouseLook;

    private CharacterController _controller;

    private Transform _direction;

    private float _stepCycle;

    private float _nextStep;

    private bool _canMove;

    private bool _inventoryAvailable;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _mouseLook = GetComponentInChildren<MouseLook>();
        _audioSource = GetComponent<AudioSource>();
        _direction = transform.GetChild(0);
        _stepCycle = 0f;
        _nextStep = _stepCycle / 2f;
        _canMove = true;
        _inventoryAvailable = true;

        StartCoroutine(ShowTutorialDialogue());
    }

    // Update is called once per frame
    void Update()
    {
        if (_inventoryAvailable && Input.GetButtonDown("Inventory"))
        {
            if (InventoryUIController.ShowingInventory)
            {
                InventoryUIController.InvokeHideInventory();
                _mouseLook.SetCanLook(true);
                SetCanMove(true);
            }
            else
            {
                InventoryUIController.InvokeShowInventory();
                _mouseLook.SetCanLook(false);
                SetCanMove(false);
            }
        }

        if (!_canMove) return;

        _controller.Move(Vector3.down * gravity * Time.deltaTime);
        MovePlayer();
    }

    private void FixedUpdate()
    {
        ProgressStepCylce(speed);
    }

    private IEnumerator ShowTutorialDialogue()
    {
        yield return null;

        DialogueController.InvokeShowDialogueEvent("I feel like I can press <color=\"red\">Tab<color=\"white\"> to access my inventory...", 5f);
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        _controller.Move(_direction.right * horizontal + _direction.forward * vertical);
    }

    private void ProgressStepCylce(float speed)
    {
        if (_controller.velocity.sqrMagnitude > 0 &&
            (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            _stepCycle += (_controller.velocity.magnitude + (speed)) * Time.fixedDeltaTime;
        }

        if (!(_stepCycle > _nextStep))
        {
            return;
        }

        _nextStep = _stepCycle + stepInterval;

        PlayFootstepAudio();
    }

    private void PlayFootstepAudio()
    {
        int index = Random.Range(1, footstepSounds.Length);
        _audioSource.clip = footstepSounds[index];
        _audioSource.PlayOneShot(_audioSource.clip);

        footstepSounds[index] = footstepSounds[0];
        footstepSounds[0] = _audioSource.clip;
    }

    public Transform GetDirection()
    {
        return _direction;
    }

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }

    public void SetInventoryAvailable(bool inv)
    {
        _inventoryAvailable = inv;
    }
}
