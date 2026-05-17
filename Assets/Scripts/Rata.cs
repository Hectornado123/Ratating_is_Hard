using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PatrullaAB : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidadGiro = 180f;

    private NavMeshAgent agent;
    private Transform destino;
    private bool girando = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destino = puntoA;
        agent.SetDestination(destino.position);
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

        destino = (destino == puntoA) ? puntoB : puntoA;

        Vector3 direccion = destino.position - transform.position;
        direccion.y = 0;

        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);

        while (Quaternion.Angle(transform.rotation, rotacionObjetivo) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                rotacionObjetivo,
                velocidadGiro * Time.deltaTime
            );

            yield return null;
        }

        agent.SetDestination(destino.position);
        agent.isStopped = false;
        girando = false;
    }
}