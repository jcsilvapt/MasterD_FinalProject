using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoorOpen : MonoBehaviour
{
    #region References

    //Animator Reference
    private Animator animator;

    //List Holding Enemies
    [SerializeField] private List<Fabio_EnemySecondLevel> enemiesList;

    #endregion

    #region Control Variables

    private bool isActive;

    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();

        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            for (int enemy = 0; enemy < enemiesList.Count; enemy++)
            {
                if (enemiesList[enemy].GetEnemyHealth() <= 0)
                {
                    enemiesList.RemoveAt(enemy);
                }
            }

            if(enemiesList.Count == 0)
            {
                isActive = false;
                OpenSlideDoor();
            }
        }
    }

    private void OpenSlideDoor()
    {
        animator.SetBool("Open", true);
        animator.SetBool("side", true);
    }
}
