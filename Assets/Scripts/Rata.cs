using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Rata : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidadGiro = 180f;
    public float tiempoIdle = 2f;

    public Transform respawnPlayer; // punto donde reaparece

    private NavMeshAgent agent;
    private Transform destino;
    private bool girando = false;
    private Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.updateRotation = false;

        destino = puntoA;
        agent.SetDestination(destino.position);

        anim.SetBool("Caminar", true);
    }

    void Update()
    {
        if (!girando && !agent.pathPending && agent.remainingDistance <= 0.5f)
        {
            StartCoroutine(CambiarDestino());
        }
    }

    IEnumerator CambiarDestino()
    {
        girando = true;
        agent.isStopped = true;

        anim.SetBool("Caminar", false);

        yield return new WaitForSeconds(tiempoIdle);

        destino = (destino == puntoA) ? puntoB : puntoA;

        Vector3 direccion = destino.position - transform.position;
        direccion.y = 0;

        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion) * Quaternion.Euler(0, 180, 0);

        while (Quaternion.Angle(transform.rotation, rotacionObjetivo) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                rotacionObjetivo,
                velocidadGiro * Time.deltaTime
            );

            yield return null;
        }

        anim.SetBool("Caminar", true);

        agent.SetDestination(destino.position);
        agent.isStopped = false;
        girando = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ha entrado algo: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador detectado");

            other.transform.position = respawnPlayer.position;

            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector3.zero;
            }
        }
    }
}