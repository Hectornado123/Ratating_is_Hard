using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PatrullaAB : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidadGiro = 180f;
    public float tiempoIdle = 2f; // duración del idle

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

        // Idle
        anim.SetBool("Caminar", false);

        // Espera a que termine la animación idle
        yield return new WaitForSeconds(tiempoIdle);

        destino = (destino == puntoA) ? puntoB : puntoA;

        Vector3 direccion = destino.position - transform.position;
        direccion.y = 0;

        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion) * Quaternion.Euler(0, 180, 0);

        // Giro después del idle
        while (Quaternion.Angle(transform.rotation, rotacionObjetivo) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                rotacionObjetivo,
                velocidadGiro * Time.deltaTime
            );

            yield return null;
        }

        // Vuelve a caminar
        anim.SetBool("Caminar", true);

        agent.SetDestination(destino.position);
        agent.isStopped = false;
        girando = false;
    }
}