using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RangedEnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float shootForce;
    private float timer;
    private float bulletTime;

    public Transform player;

    public float speed;
    public float life;
    private float start_life;
    public Rigidbody rb;

    public WeaponSystem ws;

    public Image healthBar;

    private NavMeshAgent navMeshAgent;

    private DropOnDeath dod;

    [SerializeField] private ContadorEnemigos contadorEnemigos;
    [SerializeField] private GameManager gm;
    [SerializeField] private EnemySFX enemySFX;

    private void Start()
    {
        timer = 5f;
        shootForce = 1000f;
        gm = FindObjectOfType<GameManager>();
        contadorEnemigos = FindObjectOfType<ContadorEnemigos>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        ws = FindObjectOfType<WeaponSystem>();
        rb = GetComponent<Rigidbody>();
        dod = GetComponent<DropOnDeath>();
        enemySFX = FindObjectOfType<EnemySFX>();
        if (contadorEnemigos != null)
        {
            contadorEnemigos.AddEnemy();
        }
        start_life = 10;
        life = start_life;
    }

    void Update()
    {
        transform.LookAt(player);
        if (Vector3.Distance(this.transform.position, player.position) < 15f)
        {
            ShootPlayer();
        }
        else
        {
            //Chase Player
            navMeshAgent.destination = player.transform.position;
        }
        

        //Destroy Enemy when life = 0
        if (life <= 0)
        {
            if(gm.totalEnemies != null)
            {
                contadorEnemigos.DeleteEnemy();
                dod.Drop();
                gm.ReloadScene();
            }
            enemySFX.PlayDeadSound();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            life -= ws.damage;
            healthBar.fillAmount = life / start_life;
        }
    }

    private void ShootPlayer()
    {
        bulletTime -= Time.deltaTime;

        if (bulletTime > 0) return;

        bulletTime = timer;

        GameObject proyectile = Instantiate(bullet, firingPoint.position, firingPoint.transform.rotation);
        Rigidbody rb = proyectile.GetComponent<Rigidbody>();
        rb.AddForce(rb.transform.forward * shootForce);


    }
}
