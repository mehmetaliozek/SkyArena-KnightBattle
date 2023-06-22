using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool isPause = false;
    private float time = 0.5f;
    private void Start()
    {
        animator.speed = 0.5f;
    }
    private void Update()
    {
        if (isPause)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                ResumeAnimation();
                time = 0.5f;
            }
        }
    }

    public void PauseAnimation()
    {
        animator.speed = 0;
        isPause = true;
    }

    public void ResumeAnimation()
    {
        animator.speed = 0.5f;
        isPause = false;
    }

    public void Disable(){
        gameObject.SetActive(false);
    }
}