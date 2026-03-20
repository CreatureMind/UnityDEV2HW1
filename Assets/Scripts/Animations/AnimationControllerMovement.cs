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
        // 1. Get velocity only on the X and Z plane (ignore falling/jumping)
        Vector3 horizontalVelocity = new Vector3(agent.velocity.x, 0, agent.velocity.z);
        
        // 2. Get the actual speed magnitude
        float currentSpeed = horizontalVelocity.magnitude;

        // 3. Set the Animator parameter
        // We use a small 'Damp Time' (0.1f) to make the weight shift look natural
        anim.SetFloat(speedHash, currentSpeed, 0.1f, Time.deltaTime);

        // 4. Force rotation if the agent is moving but not turning fast enough
        if (currentSpeed > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(horizontalVelocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}