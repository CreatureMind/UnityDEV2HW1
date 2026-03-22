using UnityEngine;
using UnityEngine.AI;

public class AnimationControllerMovement : MonoBehaviour
{
    [SerializeField] private SelectedCharacterParent character;
    private NavMeshAgent agent;
    private Animator anim;
    private int speedHash;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = character.SelectedCharacterObj.GetComponent<Animator>();
        character.OnCharacterChanged += c => {
            anim = c.SelectedCharacterObj.GetComponent<Animator>();
        };
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