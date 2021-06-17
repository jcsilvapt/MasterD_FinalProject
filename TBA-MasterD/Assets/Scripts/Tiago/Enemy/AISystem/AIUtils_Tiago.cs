using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIUtils_Tiago
{
    public static bool IsChasingPlayer(Transform self, Transform target, float distanceToShoot)
    {

        //If distance inferior to the max distance, starts chasing
        if (Vector3.Distance(self.position, target.position) <= distanceToShoot)
        {
            return true;
        }

      

        return false;
    }
}
