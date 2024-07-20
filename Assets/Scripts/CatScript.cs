using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatScript : MonoBehaviour
{
    [SerializeField] private WayPointSystem wayPointSystem;
    [SerializeField] private float hopOffWaitTimer;

    // private variable
    private bool canSit;

    private PlayerScript playerScript;
    private Rigidbody rb;
    private Animator animator;

    private void Awake()
    {
        canSit = true;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && canSit)
        {
            SitOnPlayer(other.transform);
        }
    }

    void SitOnPlayer(Transform _player)
    {
        canSit = false;
        playerScript = _player.GetComponent<PlayerScript>();

        //animator.SetTrigger("Sit");

        rb.isKinematic = true;
        wayPointSystem.SetMoving(false);

        transform.position = playerScript.GetCatSitPosition().position;
        transform.SetParent(_player);
        playerScript.SetBeingSatOn(true, this);
    }

    public void HopOffPlayer(Transform _player)
    {
        playerScript = _player.GetComponent<PlayerScript>();

        rb.isKinematic = false;
        wayPointSystem.SetMoving(true);

        playerScript.SetBeingSatOn(false, null);
        transform.SetParent(null);

        wayPointSystem.SelectNextWayPoint();

        StartCoroutine(HopOffWait());
    }

    IEnumerator HopOffWait()
    {
        canSit = false;
        yield return new WaitForSeconds(hopOffWaitTimer);
        canSit = true;
    }
}