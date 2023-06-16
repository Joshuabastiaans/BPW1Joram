using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchAnimation : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController controller1;
    public RuntimeAnimatorController controller2;
    private RuntimeAnimatorController currentController;

    void Start()
    {
        currentController = animator.runtimeAnimatorController;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToController(controller1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToController(controller2);
        }
    }
    public void SwitchAnimator(int Weapon)
    {
        switch (Weapon)
        {
            case 0:
                SwitchToController(controller1);
                break;
            case 1:
                SwitchToController(controller2);
                break;
        }
    }
    void SwitchToController(RuntimeAnimatorController controller)
    {
        animator.runtimeAnimatorController = controller;
        currentController = controller;
    }
}