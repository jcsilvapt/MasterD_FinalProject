using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabio_AIManager : MonoBehaviour
{
    #region References

    //Second Level AI Array Reference
    [SerializeField] private Fabio_EnemySecondLevel[] enemies;

    //Player Reference
    [SerializeField] private Transform player;

    #endregion

    #region Control Variables

    #region Should AI Manager Be Active

    [SerializeField] private bool activeAIManager;

    private int numberOfEnemiesAlive;

    #endregion

    #region Detected Player

    private bool hasDetectedPlayer;

    private bool hasVisionOnPlayer;

    #endregion

    #region Time to Chase the Player

    private bool isChasingThePlayer;

    [SerializeField] private float timeUntilStartsChasingThePlayer;

    private float currentTimerChasingThePlayer;

    #endregion

    #region Time Between Shooting Sprays

    private bool isShootingThePlayer;

    [SerializeField] private float timeBetweenShootingSprays;

    private float currentTimerBetweenShootingSprays;

    #endregion

    #endregion

    private void Start()
    {
        activeAIManager = true;
        numberOfEnemiesAlive = enemies.Length;
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
            if (hasVisionOnPlayer)
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

                    if (enemiesWithVisionOnPlayer.Count == 1)
                    {
                        enemiesWithVisionOnPlayer[0].SetShooting();
                    }
                    else if (enemiesWithVisionOnPlayer.Count < 3)
                    {
                        float[] distancesToPlayer = new float[enemiesWithVisionOnPlayer.Count];

                        for (int enemyIndex = 0; enemyIndex < enemiesWithVisionOnPlayer.Count; enemyIndex++)
                        {
                            distancesToPlayer[enemyIndex] = Vector3.Distance(enemiesWithVisionOnPlayer[enemyIndex].transform.position, player.position);
                        }

                        if (distancesToPlayer[0] > distancesToPlayer[1])
                        {
                            enemiesWithVisionOnPlayer[1].SetShooting();
                        }
                        else
                        {
                            enemiesWithVisionOnPlayer[0].SetShooting();
                        }
                    }
                    else if (enemiesWithVisionOnPlayer.Count < 6)
                    {
                        int howManyWillShoot = Random.Range(0, 101);

                        if (howManyWillShoot < 50)
                        {
                            enemiesWithVisionOnPlayer[Random.Range(0, enemiesWithVisionOnPlayer.Count)].SetShooting();
                        }
                        else if (howManyWillShoot < 90)
                        {
                            int randomEnemy1 = 0;
                            int randomEnemy2 = 0;

                            while (randomEnemy1 == randomEnemy2)
                            {
                                randomEnemy1 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                                randomEnemy2 = Random.Range(0, enemiesWithVisionOnPlayer.Count);

                            }

                            enemiesWithVisionOnPlayer[randomEnemy1].SetShooting();
                            enemiesWithVisionOnPlayer[randomEnemy2].SetShooting();
                        }
                        else
                        {
                            int randomEnemy1 = 0;
                            int randomEnemy2 = 0;
                            int randomEnemy3 = 0;

                            while (randomEnemy1 == randomEnemy2 && randomEnemy1 == randomEnemy3 && randomEnemy2 == randomEnemy3)
                            {
                                randomEnemy1 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                                randomEnemy2 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                                randomEnemy3 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                            }

                            enemiesWithVisionOnPlayer[randomEnemy1].SetShooting();
                            enemiesWithVisionOnPlayer[randomEnemy2].SetShooting();
                            enemiesWithVisionOnPlayer[randomEnemy3].SetShooting();
                        }
                    }
                    else
                    {
                        int howManyWillShoot = Random.Range(0, 101);

                        if (howManyWillShoot < 50)
                        {
                            int randomEnemy1 = 0;
                            int randomEnemy2 = 0;

                            while (randomEnemy1 == randomEnemy2)
                            {
                                randomEnemy1 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                                randomEnemy2 = Random.Range(0, enemiesWithVisionOnPlayer.Count);

                            }

                            enemiesWithVisionOnPlayer[randomEnemy1].SetShooting();
                            enemiesWithVisionOnPlayer[randomEnemy2].SetShooting();
                        }
                        else if (howManyWillShoot < 90)
                        {
                            int randomEnemy1 = 0;
                            int randomEnemy2 = 0;
                            int randomEnemy3 = 0;

                            while (randomEnemy1 == randomEnemy2 && randomEnemy1 == randomEnemy3 && randomEnemy2 == randomEnemy3)
                            {
                                randomEnemy1 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                                randomEnemy2 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                                randomEnemy3 = Random.Range(0, enemiesWithVisionOnPlayer.Count);
                            }

                            enemiesWithVisionOnPlayer[randomEnemy1].SetShooting();
                            enemiesWithVisionOnPlayer[randomEnemy2].SetShooting();
                            enemiesWithVisionOnPlayer[randomEnemy3].SetShooting();
                        }
                        else
                        {
                            enemiesWithVisionOnPlayer[Random.Range(0, enemiesWithVisionOnPlayer.Count)].SetShooting();
                        }
                    }

                    isShootingThePlayer = false;
                    currentTimerBetweenShootingSprays = 0;
                }
            }
        }
    }

    public void PlayerDetected()
    {
        hasDetectedPlayer = true;
        BecomeAware();
    }

    public void BecomeAware()
    {
        foreach (Fabio_EnemySecondLevel enemy in enemies)
        {
            enemy.gameObject.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head/Spot Light").gameObject.SetActive(true);
            enemy.SetAware();
        }
    }

    public void HasVisionOnPlayer()
    {
        hasVisionOnPlayer = true;
    }

    public void HitPlayer()
    {
        foreach (Fabio_EnemySecondLevel enemy in enemies)
        {
            enemy.SetMissAllShots();
        }
    }

    public void UnitKilled(Fabio_EnemySecondLevel unitKilled)
    {
        numberOfEnemiesAlive--;

        if(numberOfEnemiesAlive <= 0)
        {
            activeAIManager = false;
            return;
        }

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
    }
}