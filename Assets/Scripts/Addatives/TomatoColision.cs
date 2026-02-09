using System;
using UnityEngine;
using System.Collections;

public class TomatoCollision : MonoBehaviour
{
    [Header("Settings")] [Tooltip("Time in seconds before tomato is destroyed")] [SerializeField]
    private float destroyTime = 5f;

    [Header("Effects")] [Tooltip("The Particle System for the tomato splash")] [SerializeField]
    private ParticleSystem splashParticles;

    [Header("Components")] [SerializeField]
    private MeshRenderer tomatoMesh;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider tomatoCollider;

    public static event Action OnTomatoHit;

    private bool hasCollided = false;

    private void Start()
    {
        StartCoroutine(DestroyTomato());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;
        OnTomatoHit?.Invoke();
        StartCoroutine(SplashRoutine());
    }

    private IEnumerator SplashRoutine()
    {
        hasCollided = true;
        rb.isKinematic = true;
        if (tomatoMesh) tomatoMesh.enabled = false;
        if (tomatoCollider) tomatoCollider.enabled = false;
        if (splashParticles)
        {
            splashParticles.Play();
            yield return new WaitForSeconds(splashParticles.main.duration);
        }
        else
        {
            yield return null;
        }

        Destroy(gameObject);
    }


    private IEnumerator DestroyTomato()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}