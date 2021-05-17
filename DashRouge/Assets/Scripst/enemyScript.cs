using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using System;
using UnityEngine.AI;

public class enemyScript : MonoBehaviour
{
    public PlayerBehaviour player;
    NavMeshAgent agent;

    public double health = 2;
    private double attackDamage = 3.1;
    private double aggroRange = 20;
    private double attackRange = 3;
    private double attackRate = 1;
    private double attackCountdown = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("player").Length != 0)
            player = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerBehaviour>();

        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("Move", 0, .5f);
    }

    // Update is called once per frame
    void Update()
    {
        double distance;
        if (health <= 0)
        {
            transform.tag = "dead";
            Destroy(gameObject);
        }
        
        if (GameObject.FindGameObjectsWithTag("player").Length != 0)
            distance = Vector3.Distance(transform.position, player.transform.position);
        else
            distance = attackRange + 1;
        if (attackCountdown <= 0 && distance <= attackRange)
        {
            Attack();
            attackCountdown = 1 / attackRate;
        }
        attackCountdown -= Time.deltaTime;
        if (distance < attackRange)
        {
            Stop();
        }
    }

    public void takeDamage(double damage)
    {
        health -= damage;
    }

    void Attack()
    {
        //Debug.Log("Enemy attacks for " + attackDamage);
        player.takeDamage(attackDamage);
    }

    
    void Move()
    {
        double distanceToPlayer;
        if (GameObject.FindGameObjectsWithTag("player").Length != 0)
        { 
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= aggroRange)
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            }
        }
        else
            Stop();
    }

    void Stop()
    {
        agent.isStopped = true;
    }
}
