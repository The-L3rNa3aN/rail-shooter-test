using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public Transform cameraContainer;
    public float pVelocity = 5f;
    public bool isWalking = true;
    public bool isHoldingFire = false;
    private CharacterController controller;
    private Vector3 targetVector;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (isWalking) controller.Move(Time.deltaTime * pVelocity * targetVector);
    }

    public void SetPlayerTarget(Transform _target) => targetVector = (_target.position - transform.position).normalized;

    public void NodeAction(NodeListItem action)
    {
        switch(action.eventType)
        {
            case Util.EventType.look:
                StartCoroutine(Action_Look(action.param_v, action.duration));
                break;
    
            case Util.EventType.walk:
                //isWalking = true;
                StartCoroutine(Action_Walk(true));
                pVelocity = action.param_f;
                break;
            
            case Util.EventType.stop:
                //isWalking = false;
                StartCoroutine(Action_Walk(false));
                break;

            case Util.EventType.hold:
                //isHoldingFire = true;
                StartCoroutine(Action_HoldFire(true));
                break;
    
            case Util.EventType.open:
                //isHoldingFire = false;
                StartCoroutine(Action_HoldFire(false));
                break;

            case Util.EventType.wait:
                StartCoroutine(Action_Wait(action.duration));
                break;
        }
    }

    public IEnumerator Action_Look(Vector3 v, float f)
    {
        float elapsedTime = 0f;
        Quaternion _start = cameraContainer.rotation;
        Quaternion _target = Quaternion.Euler(v);

        while (elapsedTime < f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / f);
            float t2 = Mathf.SmoothStep(0f, 1f, t);
            cameraContainer.rotation = Quaternion.Slerp(_start, _target, t2);
            yield return null;
        }

        cameraContainer.rotation = _target;
    }

    public IEnumerator Action_Wait(float f) { yield return new WaitForSeconds(f); }

    public IEnumerator Action_Walk(bool b)
    {
        isWalking = b;
        yield return null;
    }

    public IEnumerator Action_HoldFire(bool b)
    {
        isHoldingFire = b;
        yield return null;
    }

    ///OTHER METHODS: -
    ///1. Update() -> works like a charm but can be performance intensive.
    ///2. DoTween
}
