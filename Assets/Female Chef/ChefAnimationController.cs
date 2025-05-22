using UnityEngine;

public class ChefAnimationController : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))  // Example key
        {
            animator.SetTrigger("Run");
        }
    }
}
