using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class BasicEnemyAI : MonoBehaviour
{
    public Transform player;

    public float speed;
    public float life;
    private float start_life;
    public Rigidbody rb;

    public float maxDist = 10;
    public float minDist = 2;

    public WeaponSystem ws;

    public Image healthBar;

    private NavMeshAgent navMeshAgent;

    private DropOnDeath dod;

    [SerializeField] private ContadorEnemigos contadorEnemigos;
    [SerializeField] private GameManager gm;
    [SerializeField] private EnemySFX enemySFX;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        contadorEnemigos = FindObjectOfType<ContadorEnemigos>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        ws = FindObjectOfType<WeaponSystem>();
        rb = GetComponent<Rigidbody>();
        dod = GetComponent<DropOnDeath>();
        enemySFX = FindObjectOfType<EnemySFX>();
        if(contadorEnemigos != null)
        {
            contadorEnemigos.AddEnemy();
        }
        start_life = 10;
        life = start_life;
    }

    void Update()
    {
        //Chase Player
        transform.LookAt(player);
        navMeshAgent.destination = player.transform.position;

        //Destroy Enemy when life = 0
        if(life <= 0)
        {
            if (gm.totalEnemies != null)
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
        if(other.tag == "Bullet")
        {
            life -= ws.damage;
            healthBar.fillAmount = life/start_life;
        }
    }

}
