using UnityEngine;
using UnityEngine.SceneManagement;
public class PickUp : MonoBehaviour
{
    [Header("Points System")]
    public int points; 
    public int winPoints = 5; 
    
    
    void Start()
    {
        points = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (points >= winPoints)
        {
            int nivelActual = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(nivelActual + 1);
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
