using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popUpPlusFive : MonoBehaviour
{
    public static popUpPlusFive instance;
    private Animator animator;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Show()
    {
        
        animator.SetBool("plus5", true);
     
    }
    public void Hide(){
        animator.SetBool("plus5", false);
    }
}
