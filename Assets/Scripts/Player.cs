using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public Transform cameraContainer;
    public float pVelocity = 5f;
    public bool isHoldingFire = false;
    public bool willStop = false;
    public bool willJump = false;
    public bool isJumping = false;

    private CharacterController controller;
    private Vector3 targetVector;

    [Header("DEBUG ONLY")]
    public bool isWalking = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //if (isWalking) controller.Move(Time.deltaTime * pVelocity * targetVector);

        if (!isJumping)
        {
            float distFromNode = Vector3.Distance(transform.position, GameManager.Main.currentNode.position);
            float currSpeed = pVelocity;
            if (willStop && distFromNode < GameManager.Main.qDistNodes /*2f*/)
            {
                //currSpeed = pVelocity * (distFromNode / 2f);
                //currSpeed = distFromNode <= 0.1f ? 0 : pVelocity * (distFromNode / 2f);
                currSpeed = distFromNode <= 0.1f ? 0 : pVelocity * (distFromNode / GameManager.Main.qDistNodes);
            }

            if (isWalking) controller.Move(Time.deltaTime * currSpeed * targetVector);
        }
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
                    pVelocity = action.param_f;
                    if (willStop || willJump)
                    {
                        willStop = false;
                        willJump = false;
                        GameManager.Main.NextNode();
                    }
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

                case Util.EventType.jump:
                    GameManager.Main.NextNode();
                    Vector3 a = transform.position;
                    Vector3 b = GameManager.Main.currentNode.position;
                    Vector3 cp = (a + b) / 2 + Vector3.up * action.param_f;
                    float time = 0f;
                    isJumping = true;

                    while (time < action.duration)
                    {
                        time += Time.deltaTime;
                        float t = time / action.duration;
                        Vector3 p = Util.QuadraticBezierPoint(t, a, cp, b);
                        controller.Move(p - transform.position);
                        yield return null;
                    }
                    transform.position = b;
                    isJumping = false;
                    willJump = false;
                    break;
            }
            yield return new WaitForSeconds(action.duration);
        }
    }
}
