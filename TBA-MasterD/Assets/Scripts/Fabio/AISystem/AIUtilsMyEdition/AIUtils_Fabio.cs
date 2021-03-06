using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class Used to support all Common behaviours Logic
/// </summary>
public class AIUtils_Fabio {

    //Detects if Self has vision on the Target
    public static bool HasVisionOfPlayer(Transform self, Transform target, float maxDistance, int layerMask = 2) {
        // Bit shift the index of the layer (8) to get a bit mask
        layerMask = 1 << layerMask;
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask
        layerMask = ~layerMask;

        //Vector Direction from Self to Target
        Vector3 direction = (target.position) - self.position;


        //If the Target is too far away, Self can't detect it, return.
        if (direction.magnitude > maxDistance) {
            return false;
        }


        //If Target isn't within Self's Vision Range, return.
        if (Vector3.Angle(self.transform.forward, direction) > 60) {
            return false;
        }

        //Create RayCast to ensure that Self doesn't have "Vision" objects in front of the Target
        Ray palpatine = new Ray(self.position + Vector3.up, direction);
        RaycastHit hitInfo;

       
        //If Target can be seen by the RayCast and HitInfo GameObject name is the same as the Target's, return true. Else, return false.
        if (Physics.Raycast(palpatine, out hitInfo, maxDistance, layerMask)) {
            if (hitInfo.transform.tag == target.tag) {
                self.LookAt(target);
                return true;
            }
        }

        return false;
    }
}

/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIUtils {

    private static bool gotHit = false;

    /// <summary>
    /// Ok this needs to be change so far even if a wall is between enemy and target they will still follow
    /// So I'll leave comment my first atempt to HasVision.
    /// This version uses the navmeshAgent Raycast, this function is CPU lighter than the Physics RayCast. 
    /// </summary>
    /// <param name="self">Enemy Character</param>
    /// <param name="target">The target usualy its the player :)</param>
    /// <returns>If you have vision or not of the Target</returns>
    public static bool HasVisionOfPlayer(GameObject self, GameObject target) {

        LayerMask playerLayer = LayerMask.GetMask("Player");
        NavMeshAgent agent = self.GetComponent<NavMeshAgent>();
        float allowedDistance = self.GetComponent<Enemy>().distanceToView;

        if (target != null && target.activeSelf && target.GetComponent<Player>().IsAlive()) {

            Vector3 direction = target.transform.position - self.transform.position;
            float distance = direction.magnitude;
            Vector3 fw = new Vector3(0, 0, 0.5f);

            float angle = Vector3.Angle(self.transform.forward, direction);
            float enemyFieldOfView = self.GetComponent<Enemy>().getFieldofView();

            NavMeshHit hit;
            RaycastHit hitInfo;


            if (angle < enemyFieldOfView && distance <= allowedDistance) {

                // Checks in the navmesh if there's a Clear Path to the Target (but ignores line of sight directly)
                if (!agent.Raycast(target.transform.position, out hit)) {
                    return true;
                }

                if(Physics.Raycast(self.transform.position + self.transform.up, target.transform.position + target.transform.up, out hitInfo)) {

                    IDamage t = hitInfo.transform.GetComponent<IDamage>();

                    if(t != null) {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public static bool GotHit() {
        gotHit = true;

        return gotHit;
    }

    public static void ResetHit() {
        gotHit = false;
    }

    public static bool HasHeardThePlayer(GameObject self, GameObject target, GameObject weapon) {
        Animator anim = target.GetComponent<Animator>();
        Animator weaponAnim = weapon.GetComponent<Animator>();
        if (target != null && target.activeSelf) {
            float distance = AIUtils.DistanceToTarget(self, target);
            if (distance < 50 && weaponAnim.GetBool("isFiring")) {
                return true;
            }
            if (distance <= 10 && anim.GetBool("isRunning")) {
                return true;
            } else if (distance < 5 && anim.GetBool("isWalking")) {
                return true;
            } else {
                return false;
            }
        }
        return false;
    }


    private static float DistanceToTarget(GameObject self, GameObject target) {
        NavMeshAgent nav = self.GetComponent<NavMeshAgent>();
        NavMeshPath path = new NavMeshPath();
        Vector3 targetPosition = target.transform.position;

        if (nav.enabled) {
            nav.CalculatePath(targetPosition, path);
        } else {
            throw new System.Exception("No nav Mesh");
        }

        Vector3[] pathToTarget = new Vector3[path.corners.Length + 2];

        pathToTarget[0] = self.transform.position;
        pathToTarget[pathToTarget.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++) {
            pathToTarget[i + 1] = path.corners[i];
        }

        float pathLenght = 0.0f;

        for (int i = 0; i < pathToTarget.Length - 1; i++) {
            pathLenght += Vector3.Distance(pathToTarget[i], pathToTarget[i + 1]);
        }

        return pathLenght;
    }

}
*/