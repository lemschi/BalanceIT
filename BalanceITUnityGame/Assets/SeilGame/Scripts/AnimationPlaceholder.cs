using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlaceholder : MonoBehaviour
{
    [SerializeField] private GameObject Sphere;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("LevelFinished"))
        {
            Sphere.SetActive(true);
        }
        else
        {
            Sphere.SetActive(false);
        }
    }
}
