using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
    NavMeshAgent navMeshAgent;

    public GameObject enemyDeathParticles;
    Transform player;

    Rigidbody[] ragdollRigidbodies;
    BoxCollider[] ragdollBoxColliders;
    SphereCollider[] ragdollSphereColliders;
    CapsuleCollider[] ragdollCapsuleColliders;
    bool collided = false;

    // Start is called before the first frame update
    void Start()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollBoxColliders = GetComponentsInChildren<BoxCollider>();
        ragdollSphereColliders = GetComponentsInChildren<SphereCollider>();
        ragdollCapsuleColliders = GetComponentsInChildren<CapsuleCollider>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        collided = false;
        foreach (Rigidbody ragdollBodies in ragdollRigidbodies)
        {
            ragdollBodies.isKinematic = true;
            ragdollBodies.useGravity = false;
        }
        foreach (BoxCollider ragdollBox in ragdollBoxColliders)
        {
            ragdollBox.enabled = false;
        }
        foreach (CapsuleCollider ragdollCapsule in ragdollCapsuleColliders)
        {
            ragdollCapsule.enabled = false;
        }
        foreach (SphereCollider ragdollSphere in ragdollSphereColliders)
        {
            ragdollSphere.enabled = false;
        }
        rb.isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = true;
        
        transform.rotation = Quaternion.Euler(0, 180f, 0f);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!collided && !PlayerController.instance.isDead)
        {
            //if(Vector3.Distance(transform.position, player.position) < 5)
            //{
                navMeshAgent.SetDestination(player.position);

            //}

        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collided = true;
            navMeshAgent.enabled = false;
            rb.AddForce(transform.up * 1250);
            anim.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            //rb.AddForce(transform.up * 500f);
            anim.SetTrigger("Dead");


            foreach (Rigidbody ragdollBodies in ragdollRigidbodies)
            {
                ragdollBodies.isKinematic = false;
                ragdollBodies.useGravity = true;
                ragdollBodies.AddForce(transform.up * 1250);
                //ragdollBodies.AddForce(transform.right * 500);
            }
            foreach (BoxCollider ragdollBox in ragdollBoxColliders)
            {
                ragdollBox.enabled = true;
            }
            foreach (CapsuleCollider ragdollCapsule in ragdollCapsuleColliders)
            {
                ragdollCapsule.enabled = true;
                GetComponent<CapsuleCollider>().enabled = false;
            }
            foreach (SphereCollider ragdollSphere in ragdollSphereColliders)
            {
                ragdollSphere.enabled = true;
            }
            //StartCoroutine(EnablingRagdollComponents());
            StartCoroutine(DestroyingEnemy());
            GetComponent<CapsuleCollider>().enabled = false; 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //GetComponent<CapsuleCollider>().isTrigger = false;
            navMeshAgent.enabled = false;
            rb.AddForce(transform.up * 400f);
            rb.useGravity = true;
            GetComponent<CapsuleCollider>().enabled = false;

            StartCoroutine(DestroyingEnemy());
        }
    }
    IEnumerator EnablingRagdollComponents()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (Rigidbody ragdollBodies in ragdollRigidbodies)
        {
            ragdollBodies.isKinematic = false;
            ragdollBodies.useGravity = true;
        }
        foreach (BoxCollider ragdollBox in ragdollBoxColliders)
        {
            ragdollBox.enabled = true;
        }
        foreach (CapsuleCollider ragdollCapsule in ragdollCapsuleColliders)
        {
            ragdollCapsule.enabled = true;
            GetComponent<CapsuleCollider>().enabled = false;
        }
        foreach (SphereCollider ragdollSphere in ragdollSphereColliders)
        {
            ragdollSphere.enabled = true;
        }
    }
    IEnumerator DestroyingEnemy()
    {
        yield return new WaitForSeconds(0.5f);
        //ObjectPooling.instance.SpawnFromPool("BloodPaticles", transform.position, transform.rotation);
        Instantiate(enemyDeathParticles, transform.position, transform.rotation);
        Destroy(gameObject);

    }
}
