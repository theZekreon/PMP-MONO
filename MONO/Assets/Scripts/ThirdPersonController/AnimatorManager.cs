using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator Animator;
    int _horizontal;
    int _vertical;

    void Awake()
    {
        Animator = GetComponent<Animator>();

        // Referencing animator values for horizontal and vertical (parameters below)
        _horizontal = Animator.StringToHash("Horizontal");
        _vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        Animator.SetBool("isInteracting", isInteracting);
        Animator.CrossFade(targetAnimation, 0.2f);
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        // Animation Snapping (optional, timestamp: 7:54)
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (horizontalMovement > 0  && horizontalMovement < 0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            snappedHorizontal = 1f;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            snappedHorizontal = -1f;
        }
        else
        {
            snappedHorizontal = 0;
        }

        // Didn't know that region was a thing, good to know
        #endregion
        #region Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            snappedVertical = 1f;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            snappedVertical = -1f;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        if (isSprinting) // Change one last time before passing off to animator
        { 
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2;
        }

        Animator.SetFloat(_horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        Animator.SetFloat(_vertical, snappedVertical, 0.1f, Time.deltaTime);
    }
}
