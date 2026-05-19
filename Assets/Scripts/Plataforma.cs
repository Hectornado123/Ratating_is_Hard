using UnityEngine;
using System.Collections;

public class Plataforma : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 2f;
    public float tiempoEspera = 1f;

    private Transform objetivo;
    private bool esperando = false;

    void Start()
    {
        objetivo = puntoB;
    }

    void Update()
    {
        if (!esperando)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                objetivo.position,
                velocidad * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, objetivo.position) < 0.01f)
            {
                StartCoroutine(EsperarYCambiar());
            }
        }
    }

    IEnumerator EsperarYCambiar()
    {
        esperando = true;

        yield return new WaitForSeconds(tiempoEspera);

        objetivo = (objetivo == puntoA) ? puntoB : puntoA;

        esperando = false;
    }
}