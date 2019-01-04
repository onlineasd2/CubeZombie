using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudView : MonoBehaviour {

    public PersonController personController = new PersonController();
    [Space]

    public Animator animator = new Animator();
    [Space]

    public Text storeCapacityUI;

    void Start () {
        storeCapacityUI.text = personController.storeCapacity.ToString();
	}
	
	void Update () {        
        AnimationCountBullet(CheckChange());
    }

    bool CheckChange ()
    {
        if (storeCapacityUI.text != personController.storeCapacity.ToString())
        {
            storeCapacityUI.text = personController.storeCapacity.ToString();
            return true;
        }
        else
        {
            return false;
        }
    } 

    void AnimationCountBullet(bool check)
    {
        if (Input.GetMouseButton(0))
            animator.SetBool("ChangeCount", true);
        else
            animator.SetBool("ChangeCount", false);
    }
}
