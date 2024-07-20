using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyScript : MonoBehaviour
{

    [SerializeField] private WayPointSystem wayPointSystem;

    [SerializeField] private float slapWaitTimer;
    [SerializeField] private AudioClip babySFX;

    // private variable
    private bool canSlap;

    private PlayerScript playerScript;
    private Rigidbody rb;
    private AudioSource audioSource;

    private void Awake()
    {
        canSlap = true;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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
        audioSource.PlayOneShot(babySFX);

        Vector3 direction = (transform.position - _player.position).normalized;
        direction = new Vector3(direction.x, -0.3f, direction.z);

        _player.GetComponent<PlayerScript>().Dump();

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