using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public Transform cameraContainer;
    public float pVelocity = 5f;
    public bool isWalking = true;
    public bool isHoldingFire = false;
    public bool willStop = false;
    private CharacterController controller;
    private Vector3 targetVector;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //if (isWalking) controller.Move(Time.deltaTime * pVelocity * targetVector);

        float distFromNode = Vector3.Distance(transform.position, GameManager.Main.currentNode.position);
        float currSpeed = pVelocity;
        if (willStop && distFromNode < 2f)
        {
            currSpeed = pVelocity * (distFromNode / 2f);
            //willStop = false;
            //GameManager.Main.NextNode();
        }

        controller.Move(Time.deltaTime * currSpeed * targetVector);
    }

    public void SetPlayerTarget(Transform _target) => targetVector = (_target.position - transform.position).normalized;

    public IEnumerator ParseNodeActions(List<NodeListItem> actions)
    {
        foreach (var action in actions)
        {
            switch(action.eventType)
            {
                case Util.EventType.look:
                    float elapsedTime = 0f;
                    Quaternion _start = cameraContainer.rotation;
                    Quaternion _target = Quaternion.Euler(action.param_v);

                    while (elapsedTime < action.duration)
                    {
                        elapsedTime += Time.deltaTime;
                        float t = Mathf.Clamp01(elapsedTime / action.duration);
                        float t2 = Mathf.SmoothStep(0f, 1f, t);
                        cameraContainer.rotation = Quaternion.Slerp(_start, _target, t2);
                        yield return null;
                    }

                    cameraContainer.rotation = _target;
                    break;

                case Util.EventType.walk:
                    isWalking = true;
                    willStop = false;
                    pVelocity = action.param_f;
                    GameManager.Main.NextNode();
                    break;

                case Util.EventType.stop:
                    //isWalking = false;
                    break;

                case Util.EventType.hold:
                    isHoldingFire = true;
                    break;

                case Util.EventType.open:
                    isHoldingFire = false;
                    break;
            }
            yield return new WaitForSeconds(action.duration);
        }
    }

    ///OTHER METHODS: -
    ///1. Update() -> works like a charm but can be performance intensive.
    ///2. DoTween
}
