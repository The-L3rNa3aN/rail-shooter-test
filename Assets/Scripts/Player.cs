using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public Transform cameraContainer;
    public float pVelocity = 5f;
    public bool isHoldingFire = false;
    public bool willStop = false;
    public bool willJump = false;
    public bool isJumping = false;
    public bool crouchTest = false;

    private CharacterController controller;
    [SerializeField] private Vector3 targetVector;
    private Vector3 targetScale;

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
            Vector3 xz = new(GameManager.Main.currentNode.position.x, transform.position.y, GameManager.Main.currentNode.position.z);
            float distFromNode = Vector3.Distance(transform.position, xz);
            float currSpeed = pVelocity;
            if (willStop && distFromNode < GameManager.Main.qDistNodes) currSpeed = distFromNode <= 0.1f ? 0 : pVelocity * (distFromNode / GameManager.Main.qDistNodes);

            if (isWalking) controller.Move(Time.deltaTime * currSpeed * targetVector);
        }
    }

    //public void SetPlayerTarget(Transform _target) => targetVector = new Vector3(_target.position.x - transform.position.x, transform.position.y, _target.position.z - transform.position.z).normalized;
    public void SetPlayerTarget(Transform _target) => targetVector = new Vector3(_target.position.x - transform.position.x, _target.position.y - transform.position.y, _target.position.z - transform.position.z).normalized;

    public IEnumerator CrouchScale(Vector3 v, float dur)
    {
        float e = 0f;
        Vector3 current = transform.localScale;
        Vector3 yVel = Vector3.up * -10f;

        while (e < dur)
        {
            e += Time.deltaTime;
            float t = Mathf.Clamp01(e / dur);
            float t2 = Mathf.SmoothStep(0f, 1f, t);
            transform.localScale = Vector3.Lerp(current, v, t2);

            controller.Move(Time.deltaTime * yVel);             //Applying a downward force to prevent the player from floating after crouching.
            yield return null;
        }

        transform.localScale = v;
    }

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
                    willJump = false;
                    if (willStop)
                    {
                        willStop = false;
                        //willJump = false;
                        GameManager.Main.NextNode();
                    }
                    break;

                case Util.EventType.stop:
                    //isWalking = false;
                    //willStop = isWalking ? true : false;
                    break;

                case Util.EventType.hold:
                    isHoldingFire = true;
                    break;

                case Util.EventType.open:
                    isHoldingFire = false;
                    break;

                case Util.EventType.jump:
                    GameManager.Main.NextNode();
                    //willJump = false;
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

                case Util.EventType.sqat:
                    targetScale = new(1f, action.param_f, 1f);
                    crouchTest = transform.localScale.y > action.param_f;
                    break;
            }
            if(action.eventType == Util.EventType.sqat)
                yield return StartCoroutine(CrouchScale(targetScale, action.duration));
            else
                yield return new WaitForSeconds(action.duration);
        }
    }
}
