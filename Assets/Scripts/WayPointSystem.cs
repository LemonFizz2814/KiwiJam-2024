using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointSystem : MonoBehaviour
{
    [SerializeField] private Transform wayPointParent;
    [SerializeField] private Animator animator;
    [Header("Variables")]
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;

    // private variables
    private int wayPointIndex;
    private bool moving;

    private Rigidbody rb;
    private Transform targetWayPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        moving = true;
        SelectNextWayPoint();
    }

    private void Update()
    {
        if (moving)
        {
            MoveToWayPoint();
            ReachedTargetCheck();
        }
    }

    public void SelectNextWayPoint()
    {
        wayPointIndex = (int)Mathf.Repeat(wayPointIndex + 1, wayPointParent.childCount);
        targetWayPoint = wayPointParent.GetChild(wayPointIndex);
    }

    void MoveToWayPoint()
    {
        transform.LookAt(targetWayPoint);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        rb.velocity += transform.forward * Time.deltaTime * speed;
    }

    void ReachedTargetCheck()
    {
        if (Vector3.Distance(transform.position, targetWayPoint.position) <= 0.3f)
        {
            StartCoroutine(Waiting());
        }
    }

    private IEnumerator Waiting()
    {
        SetMoving(false);
        //animator.SetTrigger("Sit");
        yield return new WaitForSeconds(waitTime);
        SelectNextWayPoint();
        SetMoving(true);
    }

    public void SetMoving(bool _moving)
    {
        moving = _moving;

        if(animator)
            animator.SetBool("Walking", _moving);
        if (_moving)
            StopAllCoroutines();
    }
}