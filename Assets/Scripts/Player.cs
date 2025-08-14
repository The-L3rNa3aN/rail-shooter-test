using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public Transform cameraContainer;
    public float pVelocity = 5f;
    private CharacterController controller;
    private Vector3 targetVector;

    [Header("EventType.look")]
    private Quaternion targetRotation;
    private float rotationDuration;
    private bool startRot;
    private float elapsedTime = 0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        controller.Move(Time.deltaTime * pVelocity * targetVector);

        Action_Look2(); // EventType.look using Update()
    }

    public void SetPlayerTarget(Transform _target) => targetVector = (_target.position - transform.position).normalized;

    public void NodeAction(NodeListItem action)
    {
        switch(action.eventType)
        {
            case Util.EventType.look:
                //StartCoroutine(Action_Look(action.param_v, action.duration));
                targetRotation = Quaternion.Euler(action.param_v);
                rotationDuration = action.duration;
                startRot = true;
                break;
    
            case Util.EventType.walk:
                break;
            
            case Util.EventType.stop:
            case Util.EventType.hold:
                break;
    
            case Util.EventType.open:
                break;
        }
    }

    //Works but it feels stiff.
    public IEnumerator Action_Look(Vector3 v, float f)
    {
        float elapsedTime = 0f;
        Quaternion targetRotation = Quaternion.Euler(v);

        while (elapsedTime < f)
        {
            float t = elapsedTime / f;
            cameraContainer.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void Action_Look2()
    {
        if(startRot)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / rotationDuration);
            cameraContainer.rotation = Quaternion.Lerp(cameraContainer.rotation, targetRotation, t);
            //Debug.Log($"elapsedTime: {elapsedTime}, t: {t}");
            Debug.Log($"camera Rotation: {cameraContainer.rotation}, t: {t}");    //It works but weirdly. How is that?

            if (t >= 1f)
            {
                startRot = false;
                elapsedTime = 0f;
            }
        }
    }

    ///OTHER METHODS: -
    ///1. Update()
    ///2. DoTween
}
