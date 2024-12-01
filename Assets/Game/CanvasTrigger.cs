using UnityEngine;

public class CanvasTrigger : MonoBehaviour
{
    [Header("Canvas to Show")]
    public GameObject canvasToShow; // Odkaz na Canvas, který se má zobrazit

    private void Start()
    {
        // Ujistíme se, že Canvas je na zaèátku vypnutý
        if (canvasToShow != null)
        {
            canvasToShow.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Zkontrolujeme, zda se do triggeru dostal hráè (tag "Player")
        if (other.CompareTag("Player"))
        {
            // Zobrazíme Canvas
            if (canvasToShow != null)
            {
                canvasToShow.SetActive(true);

                // Zastavíme èas, pokud je potøeba
                Time.timeScale = 0f;

                Debug.Log("Player entered the trigger zone. Canvas shown.");
            }
        }
    }
}