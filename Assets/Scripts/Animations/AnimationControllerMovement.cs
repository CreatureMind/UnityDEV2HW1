using UnityEngine;
using UnityEngine.AI;

public class AnimationControllerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private int speedHash;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.updateRotation = true; 
        agent.updatePosition = true;
        speedHash = Animator.StringToHash("Speed");
    }

    void Update()
    {
        Vector3 horizontalVelocity = new Vector3(agent.velocity.x, 0, agent.velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;
        anim.SetFloat(speedHash, currentSpeed, 0.1f, Time.deltaTime);
        if (currentSpeed > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(horizontalVelocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}