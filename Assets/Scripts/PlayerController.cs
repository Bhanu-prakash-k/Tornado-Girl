using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float speed = 3f;
    public float maxSpeed = 5f;
    private float startingSpeed;

    public float moveSpeed = 3f;

    public float energy = 100f;

    public Slider energySlider;
    public TMP_Text energyValueText;

    public bool canStart = false;
    [HideInInspector]
    public bool isLevelFinished = false;
    public bool isDead = false;

    public GameObject damageText;
    public Transform damageTextPosition;

    public Animator anim;
    public Rigidbody rb;

    public VariableJoystick variableJoystick;

    [SerializeField] private ParticleSystem tornado;
    [SerializeField] private ParticleSystem energyParticles;
    //ParticleSystem tornadoChild;

    ParticleSystem.VelocityOverLifetimeModule tornadoVelocity;

    public int totalEnemies;

    [SerializeField] private AudioSource zombieDeathSound;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource collectedSound;
    //ParticleSystem.VelocityOverLifetimeModule tornadoChildVelocity;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        startingSpeed = speed;
        energySlider.value = energy;
        energyValueText.text = energySlider.value.ToString();

        //tornado = transform.GetChild(1).GetComponent<ParticleSystem>();
        //tornadoChild = transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>();

        tornadoVelocity = tornado.velocityOverLifetime;
        //tornadoChildVelocity = tornadoChild.velocityOverLifetime;
        tornadoVelocity.yMultiplier = 2f;
        //tornadoChildVelocity.yMultiplier = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (transform.position.x > 10)
            {
                transform.position = new Vector3(10, transform.position.y, transform.position.z);
            }
            if (transform.position.x < -10)
            {
                transform.position = new Vector3(-10, transform.position.y, transform.position.z);
            }
            if (transform.position.z > 24)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 24);
            }
            if (transform.position.z < -5)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -5);
            }
            if (Input.GetMouseButtonDown(0))
            {
                canStart = true;
            }
            if (canStart)
            {
                //transform.position += new Vector3(variableJoystick.Horizontal, 0f, variableJoystick.Vertical) * moveSpeed * Time.deltaTime;
            }
            if (Input.GetMouseButton(0) && !isDead)
            {
                if (speed <= maxSpeed)
                {
                    speed += 10 * Time.deltaTime;
                }

                tornadoVelocity.yMultiplier = Mathf.Lerp(tornadoVelocity.yMultiplier, 2f, 0.125f);
                //tornadoChildVelocity.yMultiplier = Mathf.Lerp(tornadoChildVelocity.yMultiplier, 2f, 0.125f);
                transform.Rotate(0f, 1f * speed * Time.deltaTime, 0f);
                anim.SetBool("Run", true);
                energy -= 10 * Time.deltaTime;
            }
            if (Input.GetMouseButtonUp(0))
            {
                tornadoVelocity.yMultiplier = 0.8f;
                rb.velocity = Vector3.zero;
                //tornadoChildVelocity.yMultiplier = 0.8f;
                speed = startingSpeed;
                anim.SetBool("Run", false);
            }
            energySlider.value = energy;
            energyValueText.text = energySlider.value.ToString();
            if (energy <= 0)
            {
                isDead = true;
                Destroy(transform.GetComponent<Rigidbody>());   
                transform.GetComponent<CapsuleCollider>().enabled = false;
                transform.GetComponent<BoxCollider>().enabled = false;
                tornadoVelocity.yMultiplier = 0.8f;
                rb.velocity = Vector3.zero;
                anim.SetBool("Run", false);
                anim.SetTrigger("Death");
                StartCoroutine(LoadSameLevel());
            }
            if (totalEnemies <= 0)
            {
                Debug.Log("Level Finished");
                isDead = true;
                isLevelFinished = true;
                tornadoVelocity.yMultiplier = 0.8f;
                rb.velocity = Vector3.zero;
                anim.SetBool("Run", false);
                anim.SetTrigger("Dance");
                winSound.Play();
                StartCoroutine(LoadNextLevel());
            }
        }
    }
    void FixedUpdate()
    {
        if (!isDead && canStart)
        {
            rb.velocity = new Vector3(variableJoystick.Horizontal * moveSpeed, rb.velocity.y, variableJoystick.Vertical * moveSpeed);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //DamageIndicator indicator = Instantiate(damageText, damageTextPosition.position, Quaternion.identity).GetComponent<DamageIndicator>();
            //indicator.SetDamage(100);
            totalEnemies--;
            zombieDeathSound.Play();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //DamageIndicator indicator = Instantiate(damageText, damageTextPosition.position, Quaternion.identity).GetComponent<DamageIndicator>();
            //indicator.SetDamage(100);
            totalEnemies--;
            zombieDeathSound.Play();
            
        }
        if (other.gameObject.CompareTag("Energy"))
        {
            collectedSound.Play();
            if(energy < 90f)
            {
                energy += 10f;
            }
            else if(energy<=100f && energy > 90f)
            {
                float valueToAdd = 100 - energy;
                energy += valueToAdd;
            }
            energyParticles.Play();
            Destroy(other.gameObject);
        }
    }
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.winPanel.SetActive(true);
    }
    IEnumerator LoadSameLevel()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.losePanel.SetActive(true);
    }
}
