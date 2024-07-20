using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyScript : MonoBehaviour
{

    [SerializeField] private WayPointSystem wayPointSystem;

    [SerializeField] private float slapWaitTimer;
    [SerializeField] private float slapForce;
    [SerializeField] private Vector3 forceDirection;

    // private variable
    private bool canSlap;

    private PlayerScript playerScript;
    private Rigidbody rb;

    private void Awake()
    {
        canSlap = true;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canSlap)
        {
            SlapPlayer(other.transform);
        }
    }

    void SlapPlayer(Transform _player)
    {
        Vector3 direction = (transform.position - _player.position).normalized;
        direction = new Vector3(direction.x, -0.3f, direction.z);
        //print("direction " + direction);
        //_player.GetComponent<Rigidbody>().AddForce(-direction * slapForce, ForceMode.Impulse);
        StartCoroutine(CanSlapAgainWait());
        // TODO: animate baby slap
    }

    IEnumerator CanSlapAgainWait()
    {
        canSlap = false;
        yield return new WaitForSeconds(slapWaitTimer);
        canSlap = true;
    }
}