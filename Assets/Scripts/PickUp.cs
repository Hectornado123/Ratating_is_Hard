using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Points System")]
    public int points; //Puntuación actual del player (en juego)
    public int winPoints = 4; //Puntuación a alcanzar para completar el nivel

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        points = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (points >= winPoints)
        {

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            points += 1;
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }
}
