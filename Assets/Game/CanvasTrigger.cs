using UnityEngine;

public class CanvasTrigger : MonoBehaviour
{
    [Header("Canvas to Show")]
    public GameObject canvasToShow; // Odkaz na Canvas, kter� se m� zobrazit

    private void Start()
    {
        // Ujist�me se, �e Canvas je na za��tku vypnut�
        if (canvasToShow != null)
        {
            canvasToShow.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Zkontrolujeme, zda se do triggeru dostal hr�� (tag "Player")
        if (other.CompareTag("Player"))
        {
            // Zobraz�me Canvas
            if (canvasToShow != null)
            {
                canvasToShow.SetActive(true);

                // Zastav�me �as, pokud je pot�eba
                Time.timeScale = 0f;

                Debug.Log("Player entered the trigger zone. Canvas shown.");
            }
        }
    }
}