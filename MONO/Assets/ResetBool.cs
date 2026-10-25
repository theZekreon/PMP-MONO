using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string IsInteractingBool;
    public bool IsInteractingStatus;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(IsInteractingBool, IsInteractingStatus);
    }
}