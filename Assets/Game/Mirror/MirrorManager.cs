using UnityEngine;
using System.Collections;

public class MirrorManager : MonoBehaviour
{
    [Header("Mirror Settings")]
    public GameObject mirrorObject;                  // Objekt zrcadla
    public Transform mirrorHoldPositionTransform;    // Bod B: pozice, kde bude zrcadlo drženo pøed kamerou
    public Transform mirrorHiddenPositionTransform;  // Bod A: pozice zrcadla v kapse hráèe
    public KeyCode mirrorKey = KeyCode.F;            // Klávesa pro zobrazení zrcadla
    public float animationDuration = 0.5f;           // Délka animace v sekundách

    [Header("Animation Effects")]
    public float shakeIntensity = 0.05f;             // Intenzita tøesení
    public float shakeFrequency = 25f;               // Frekvence tøesení
    public AnimationCurve movementCurve;             // Køivka pro pohyb (easing)

    [Header("Options")]
    public bool disablePlayerMovement = true;        // Pokud je zaškrtnuto, hráè se nemùže hýbat pøi zobrazení zrcadla

    private bool mirrorActive = false;       // Stav zrcadla (zobrazeno/skryto)
    private bool isAnimating = false;        // Kontrola, zda probíhá animace

    private FirstPersonController playerController;  // Reference na skript FirstPersonController

    void Start()
    {
        // Získání reference na hráèùv kontroler
        playerController = GetComponent<FirstPersonController>();

        // Ujistìte se, že je zrcadlo na zaèátku skryté
        mirrorObject.SetActive(false);

        // Nastavení výchozí lokální pozice a rotace zrcadla na skrytou pozici (bod A)
        mirrorObject.transform.localPosition = mirrorHiddenPositionTransform.localPosition;
        mirrorObject.transform.localRotation = mirrorHiddenPositionTransform.localRotation;

        // Pokud není pøiøazena movementCurve, použij defaultní (EaseInOut)
        if (movementCurve == null)
        {
            movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        }
    }

    void Update()
    {
        // Detekce stisku klávesy
        if (Input.GetKeyDown(mirrorKey) && !isAnimating)
        {
            if (!mirrorActive)
            {
                // Zobrazení zrcadla
                StartCoroutine(ShowMirror());
            }
            else
            {
                // Skrytí zrcadla
                StartCoroutine(HideMirror());
            }
        }
    }

    IEnumerator ShowMirror()
    {
        isAnimating = true;

        if (disablePlayerMovement)
        {
            // Zakázání pohybu hráèe a kamery
            playerController.playerCanMove = false;
            playerController.cameraCanMove = false;

            // Resetování promìnných pohybu, aby se zastavila animace chùze
            playerController.isWalking = false;
        }

        // Aktivace objektu zrcadla
        mirrorObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float curveT = movementCurve.Evaluate(t);

            // Interpolace pozice s použitím køivky
            Vector3 currentPosition = Vector3.Lerp(mirrorHiddenPositionTransform.localPosition, mirrorHoldPositionTransform.localPosition, curveT);

            // Pøidání tøesení
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity * (1f - curveT);
            currentPosition += shakeOffset;

            // Interpolace základní rotace
            Quaternion baseRotation = Quaternion.Slerp(mirrorHiddenPositionTransform.localRotation, mirrorHoldPositionTransform.localRotation, curveT);

            // Pøidání 360° rotace kolem osy Y pøi vytahování
            float rotationAngle = 360f * curveT; // od 0 do 360 stupòù
            Quaternion spinRotation = Quaternion.Euler(0f, rotationAngle, 0f);

            // Kombinace základní rotace a otáèení
            Quaternion currentRotation = baseRotation * spinRotation;

            // Pøidání drobného tøesení k rotaci
            float shakeAngle = Mathf.Sin(elapsedTime * shakeFrequency) * shakeIntensity * (1f - curveT) * 10f;
            Quaternion shakeRotation = Quaternion.Euler(shakeAngle, shakeAngle, shakeAngle);
            currentRotation *= shakeRotation;

            // Aplikace pozice a rotace
            mirrorObject.transform.localPosition = currentPosition;
            mirrorObject.transform.localRotation = currentRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ujistìte se, že je zrcadlo pøesnì v cílové pozici a rotaci
        mirrorObject.transform.localPosition = mirrorHoldPositionTransform.localPosition;
        mirrorObject.transform.localRotation = mirrorHoldPositionTransform.localRotation;

        isAnimating = false;
        mirrorActive = true;
    }

    IEnumerator HideMirror()
    {
        isAnimating = true;

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float curveT = movementCurve.Evaluate(t);

            // Interpolace pozice s použitím køivky (zpìt z bodu B do bodu A)
            Vector3 currentPosition = Vector3.Lerp(mirrorHoldPositionTransform.localPosition, mirrorHiddenPositionTransform.localPosition, curveT);

            // Pøidání tøesení
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity * (1f - curveT);
            currentPosition += shakeOffset;

            // Interpolace základní rotace
            Quaternion baseRotation = Quaternion.Slerp(mirrorHoldPositionTransform.localRotation, mirrorHiddenPositionTransform.localRotation, curveT);

            // Pøidání 360° rotace kolem osy Y pøi schovávání
            float rotationAngle = 360f * (1f - curveT); // od 360 do 0 stupòù
            Quaternion spinRotation = Quaternion.Euler(0f, rotationAngle, 0f);

            // Kombinace základní rotace a otáèení
            Quaternion currentRotation = baseRotation * spinRotation;

            // Pøidání drobného tøesení k rotaci
            float shakeAngle = Mathf.Sin(elapsedTime * shakeFrequency) * shakeIntensity * (1f - curveT) * 10f;
            Quaternion shakeRotation = Quaternion.Euler(shakeAngle, shakeAngle, shakeAngle);
            currentRotation *= shakeRotation;

            // Aplikace pozice a rotace
            mirrorObject.transform.localPosition = currentPosition;
            mirrorObject.transform.localRotation = currentRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ujistìte se, že je zrcadlo pøesnì ve skryté pozici a rotaci
        mirrorObject.transform.localPosition = mirrorHiddenPositionTransform.localPosition;
        mirrorObject.transform.localRotation = mirrorHiddenPositionTransform.localRotation;

        // Deaktivace objektu zrcadla
        mirrorObject.SetActive(false);

        if (disablePlayerMovement)
        {
            // Povolení pohybu hráèe a kamery
            playerController.playerCanMove = true;
            playerController.cameraCanMove = true;
        }

        isAnimating = false;
        mirrorActive = false;
    }
}