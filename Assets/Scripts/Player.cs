using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public Transform cameraContainer;
    public Transform target;
    public float pVelocity = 5f;
    private CharacterController controller;
    private Vector3 targetVector;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        targetVector = (target.position - transform.position).normalized;
    }

    private void Update()
    {
        controller.Move(Time.deltaTime * pVelocity * targetVector);
    }

    public void SetPlayerTarget(Transform _target)
    {
        target = _target;
        targetVector = (target.position - transform.position).normalized;
    }
}