using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player
    public float movementSpeed;
    Animation anim;

    private float attackTimer;

    private bool moving;
    private bool attacking;
    private bool followingEnemy;

    //PMR
    public GameObject playerMovePoint;
    private Transform pmr;
    private bool triggeringPMR;

    //Enemy

    private bool triggeringEnemy;
    private GameObject attackingEnemy;



    // Functions
    void Start()
    {
        pmr = Instantiate(playerMovePoint.transform, this.transform.position, Quaternion.identity);
        pmr.GetComponent<BoxCollider>().enabled = false;
        anim = GetComponent<Animation>();
    }

    void Update()
    {
        //player movement
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float hitDistance = 0.0f;

        if(playerPlane.Raycast(ray, out hitDistance))
        {
            Vector3 mousePosition = ray.GetPoint(hitDistance);
            if (Input.GetMouseButtonDown(0))
            {
                moving = true;
                triggeringPMR = false;
                pmr.GetComponent<BoxCollider>().enabled = true;
                pmr.transform.position = mousePosition;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        attackingEnemy = hit.collider.gameObject;
                        followingEnemy = true;
                    }
                }
                else
                {
                    attackingEnemy = null;
                    followingEnemy = false;
                }
                
            }
        }


        if (moving)
            Move();
        else
            if (triggeringEnemy)
            Attack();
            else
            Idle();

        if (triggeringPMR)
        {
            moving = false;
        }

        if (triggeringEnemy)
            Attack();
    }

    public void Attack()
    {
        anim.CrossFade("playerAttack");
        transform.LookAt(pmr.transform);
    }

    public void Idle()
    {
        anim.CrossFade("playerIdle");
    }

    public void Move()
    {

        if (followingEnemy)
        {
            transform.position = Vector3.MoveTowards(transform.position, attackingEnemy.transform.position, movementSpeed);
            this.transform.LookAt(attackingEnemy.transform);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pmr.transform.position, movementSpeed);
            this.transform.LookAt(pmr.transform);
        }

        anim.CrossFade("playerWalk");
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PMR")
        {
            triggeringPMR = true;
            
        }

        if (other.tag == "Enemy")
        {
            triggeringEnemy = true;
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "PMR")
        {
            triggeringPMR = false;

        }

        if (other.tag == "Enemy")
        {
            triggeringEnemy = false;
            
        }
    }
}
