using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabio_AIManager : MonoBehaviour
{
    #region References

    //Level Manager Reference
    [SerializeField] private SecondLevelManager levelManager;

    //Second Level AI Array Reference
    [SerializeField] private Fabio_EnemySecondLevel[] enemies;

    //Player Reference
    [SerializeField] private Transform player;

    //Door Reference
    [SerializeField] private Transform door;

    #endregion

    #region Control Variables

    #region Should AI Manager Be Active

    [SerializeField] private bool activeAIManager;

    private int numberOfEnemiesAlive;

    #endregion

    #region Is AI Manager Working

    private bool isAIManagerWorking;

    #endregion

    #region Detected Player

    public int howManyEnemiesHaveVisionOnPlayer;

    private bool hasDetectedPlayer;

    private bool hasVisionOnPlayer;

    #endregion

    #region Time to Chase the Player

    private bool hasChasedThePlayerBefore;
    private Fabio_EnemySecondLevel lastToChase;

    private bool isChasingThePlayer;

    [SerializeField] private float timeUntilStartsChasingThePlayer;

    private float currentTimerChasingThePlayer;

    #endregion

    #region Time Between Shooting Sprays

    [SerializeField] private float timeBetweenShootingSprays;

    private float currentTimerBetweenShootingSprays;

    #endregion

    #endregion

    private void Start()
    {
        activeAIManager = true;
        numberOfEnemiesAlive = enemies.Length;

        isAIManagerWorking = false;
    }

    private void Update()
    {
        //If there's no enemy alive, this AI Manager loses functionality
        if(!activeAIManager)
        {
            return;
        }

        if (hasDetectedPlayer)
        {
            /* Logic Behind Has Vision On Player
             * 
             * If the Enemy Has Vision on the Player, the Timer For the Shooting Spray will Start.
             * When it reaches the time to shoot, the manager will collect the references of the Enemies that have vision of 
             * the player at that moment.
             * 
             * If there's only 1 enemy with vision, the manager will order him to shoot the player.
             * 
             * If there's only 2 enemies with vision, the manager will order the closest to the player to shoot.
             * 
             * If there's 5 enemies or less, the manager will decide, randomly, if one, two or three enemies shoot.
             * More likely 1, less likely 2, very unlikely 3.
             * 
             * If there's 6 enemies or more, the manager will decide, randomly, if one, two or three enemies shoot.
             * Very unlikely 1, more likely 2, less likely 3.
            */
            if (HasVisionOnPlayer())
            {
                currentTimerBetweenShootingSprays += Time.deltaTime;

                if (currentTimerBetweenShootingSprays >= timeBetweenShootingSprays)
                {
                    List<Fabio_EnemySecondLevel> enemiesWithVisionOnPlayer = new List<Fabio_EnemySecondLevel>();

                    foreach (Fabio_EnemySecondLevel enemy in enemies)
                    {
                        if (enemy.CanSeeThePlayer())
                        {
                            enemiesWithVisionOnPlayer.Add(enemy);
                        }
                    }

                    if(enemiesWithVisionOnPlayer.Count == 0)
                    {
                        return;
                    }

                    float smallerDistance = 0;
                    int closestEnemyDistanceIndex = 0;

                    for (int enemyIndex = 0; enemyIndex < enemiesWithVisionOnPlayer.Count; enemyIndex++)
                    {
                        if (enemyIndex == 0)
                        {
                            smallerDistance = Vector3.Distance(enemies[enemyIndex].transform.position, door.position);
                            closestEnemyDistanceIndex = enemyIndex;
                        }
                        else
                        {
                            if (Vector3.Distance(enemies[enemyIndex].transform.position, door.position) < smallerDistance)
                            {
                                smallerDistance = Vector3.Distance(enemies[enemyIndex].transform.position, door.position);
                                closestEnemyDistanceIndex = enemyIndex;
                            }
                        }
                    }

                    if (enemiesWithVisionOnPlayer.Count < 3)
                    {
                        enemiesWithVisionOnPlayer[closestEnemyDistanceIndex].SetShooting();
                    }
                    else if (enemiesWithVisionOnPlayer.Count < 6)
                    {
                        int howManyWillShoot = Random.Range(0, 101);

                        if (howManyWillShoot < 50)
                        {
                            enemiesWithVisionOnPlayer[closestEnemyDistanceIndex].SetShooting();
                        }
                        else if (howManyWillShoot < 90)
                        {
                            int randomEnemy1 = 0;

                            while (randomEnemy1 == closestEnemyDistanceIndex)
                            {
                                randomEnemy1 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                            }

                            enemiesWithVisionOnPlayer[closestEnemyDistanceIndex].SetShooting();
                            enemiesWithVisionOnPlayer[randomEnemy1].SetShooting();
                        }
                        else
                        {
                            int randomEnemy1 = 0;
                            int randomEnemy2 = 0;

                            while (randomEnemy1 == randomEnemy2 && randomEnemy1 == closestEnemyDistanceIndex && randomEnemy2 == closestEnemyDistanceIndex)
                            {
                                randomEnemy1 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                                randomEnemy2 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                            }

                            enemiesWithVisionOnPlayer[closestEnemyDistanceIndex].SetShooting();
                            enemiesWithVisionOnPlayer[randomEnemy1].SetShooting();
                            enemiesWithVisionOnPlayer[randomEnemy2].SetShooting();
                        }
                    }
                    else
                    {
                        int howManyWillShoot = Random.Range(0, 101);

                        if (howManyWillShoot < 50)
                        {
                            int randomEnemy1 = 0;

                            while (randomEnemy1 == closestEnemyDistanceIndex)
                            {
                                randomEnemy1 = Random.Range(0, enemiesWithVisionOnPlayer.Count);

                            }

                            enemiesWithVisionOnPlayer[closestEnemyDistanceIndex].SetShooting();
                            enemiesWithVisionOnPlayer[randomEnemy1].SetShooting();
                        }
                        else if (howManyWillShoot < 90)
                        {
                            int randomEnemy1 = 0;
                            int randomEnemy2 = 0;

                            while (randomEnemy1 == randomEnemy2 && randomEnemy1 == closestEnemyDistanceIndex && randomEnemy2 == closestEnemyDistanceIndex)
                            {
                                randomEnemy1 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                                randomEnemy2 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                            }

                            enemiesWithVisionOnPlayer[closestEnemyDistanceIndex].SetShooting();
                            enemiesWithVisionOnPlayer[randomEnemy1].SetShooting();
                            enemiesWithVisionOnPlayer[randomEnemy2].SetShooting();
                        }
                        else
                        {
                            enemiesWithVisionOnPlayer[closestEnemyDistanceIndex].SetShooting();
                        }
                    }

                    currentTimerBetweenShootingSprays = 0;
                }
            }
            else
            {
                if (!isChasingThePlayer)
                {
                    currentTimerChasingThePlayer += Time.deltaTime;

                    if(currentTimerChasingThePlayer >= timeUntilStartsChasingThePlayer)
                    {
                        if (hasChasedThePlayerBefore)
                        {
                            if(lastToChase.GetEnemyHealth() > 0)
                            {
                                lastToChase.SetChasing();
                            }
                            else
                            {
                                float smallerDistance = 0;
                                Fabio_EnemySecondLevel enemyToChase = null;

                                for (int enemyIndex = 0; enemyIndex < enemies.Length; enemyIndex++)
                                {
                                    if (enemyIndex == 0)
                                    {
                                        smallerDistance = Vector3.Distance(enemies[enemyIndex].transform.position, door.position);
                                        enemyToChase = enemies[enemyIndex];
                                    }
                                    else
                                    {
                                        if (Vector3.Distance(enemies[enemyIndex].transform.position, door.position) < smallerDistance)
                                        {
                                            smallerDistance = Vector3.Distance(enemies[enemyIndex].transform.position, door.position);
                                            enemyToChase = enemies[enemyIndex];
                                        }
                                    }
                                }

                                enemyToChase.SetChasing();
                                hasChasedThePlayerBefore = true;
                                lastToChase = enemyToChase;
                            }
                        }
                        else
                        {
                            float smallerDistance = 0;
                            Fabio_EnemySecondLevel enemyToChase = null;

                            for (int enemyIndex = 0; enemyIndex < enemies.Length; enemyIndex++)
                            {
                                if (enemyIndex == 0)
                                {
                                    smallerDistance = Vector3.Distance(enemies[enemyIndex].transform.position, door.position);
                                    enemyToChase = enemies[enemyIndex];
                                }
                                else
                                {
                                    if (Vector3.Distance(enemies[enemyIndex].transform.position, door.position) < smallerDistance)
                                    {
                                        smallerDistance = Vector3.Distance(enemies[enemyIndex].transform.position, door.position);
                                        enemyToChase = enemies[enemyIndex];
                                    }
                                }
                            }

                            enemyToChase.SetChasing();
                            hasChasedThePlayerBefore = true;
                            lastToChase = enemyToChase;
                        }

                        isChasingThePlayer = true;
                        currentTimerChasingThePlayer = 0;
                    }
                }
            }
        }
    }

    public void PlayerDetected()
    {
        hasDetectedPlayer = true;
        BecomeAware();

        isAIManagerWorking = true;

        levelManager.SetActionAudio();
    }

    public void BecomeAware()
    {
        foreach (Fabio_EnemySecondLevel enemy in enemies)
        {
            enemy.gameObject.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/Spot Light").gameObject.SetActive(true);
            enemy.SetAware();
        }
    }

    public bool HasVisionOnPlayer()
    {
        foreach(Fabio_EnemySecondLevel enemy in enemies)
        {
            if (enemy.GetIsSeeingThePlayer())
            {
                return true;
            }
        }

        return false;
    }

    public void StopChasing()
    {
        isChasingThePlayer = false;
    }

    public void HitPlayer()
    {
        foreach (Fabio_EnemySecondLevel enemy in enemies)
        {
            if(enemy.GetCurrentBehaviour() == "Shoot")
            {
                enemy.SetMissAllShots();
            }
        }
    }

    public void UnitKilled(Fabio_EnemySecondLevel unitKilled)
    {
        numberOfEnemiesAlive--;

        Fabio_EnemySecondLevel[] auxiliarEnemies = new Fabio_EnemySecondLevel[enemies.Length - 1];
        int auxiliarIndex = 0;

        for (int enemyIndex = 0; enemyIndex < enemies.Length; enemyIndex++)
        {
            if(enemies[enemyIndex] != unitKilled)
            {
                auxiliarEnemies[auxiliarIndex] = enemies[enemyIndex];
                auxiliarIndex++;
            }
        }

        enemies = auxiliarEnemies;

        isChasingThePlayer = false;

        if (numberOfEnemiesAlive <= 0)
        {
            activeAIManager = false;

            isAIManagerWorking = false;

            levelManager.SetNormalAudio();

            return;
        }
    }

    public bool GetIsAIManagerActive()
    {
        return activeAIManager;
    }

    public bool GetIsAIManagerWorking()
    {
        return isAIManagerWorking;
    }
}