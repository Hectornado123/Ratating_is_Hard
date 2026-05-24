using UnityEngine;
using System.Collections;
public class Limite : MonoBehaviour
{
    public Transform respawnPlayer;
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
