using UnityEngine;
using System.Collections;

public class MirrorManager : MonoBehaviour
{
    [Header("Mirror Settings")]
    public GameObject mirrorObject;                  // Objekt zrcadla
    public Transform mirrorHoldPositionTransform;    // Bod B: pozice, kde bude zrcadlo dr�eno p�ed kamerou
    public Transform mirrorHiddenPositionTransform;  // Bod A: pozice zrcadla v kapse hr��e
    public KeyCode mirrorKey = KeyCode.F;            // Kl�vesa pro zobrazen� zrcadla
    public float animationDuration = 0.5f;           // D�lka animace v sekund�ch

    [Header("Animation Effects")]
    public float shakeIntensity = 0.05f;             // Intenzita t�esen�
    public float shakeFrequency = 25f;               // Frekvence t�esen�
    public AnimationCurve movementCurve;             // K�ivka pro pohyb (easing)

    [Header("Options")]
    public bool disablePlayerMovement = true;        // Pokud je za�krtnuto, hr�� se nem��e h�bat p�i zobrazen� zrcadla

    private bool mirrorActive = false;       // Stav zrcadla (zobrazeno/skryto)
    private bool isAnimating = false;        // Kontrola, zda prob�h� animace

    private FirstPersonController playerController;  // Reference na skript FirstPersonController

    void Start()
    {
        // Z�sk�n� reference na hr���v kontroler
        playerController = GetComponent<FirstPersonController>();

        // Ujist�te se, �e je zrcadlo na za��tku skryt�
        mirrorObject.SetActive(false);

        // Nastaven� v�choz� lok�ln� pozice a rotace zrcadla na skrytou pozici (bod A)
        mirrorObject.transform.localPosition = mirrorHiddenPositionTransform.localPosition;
        mirrorObject.transform.localRotation = mirrorHiddenPositionTransform.localRotation;

        // Pokud nen� p�i�azena movementCurve, pou�ij defaultn� (EaseInOut)
        if (movementCurve == null)
        {
            movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        }
    }

    void Update()
    {
        // Detekce stisku kl�vesy
        if (Input.GetKeyDown(mirrorKey) && !isAnimating)
        {
            if (!mirrorActive)
            {
                // Zobrazen� zrcadla
                StartCoroutine(ShowMirror());
            }
            else
            {
                // Skryt� zrcadla
                StartCoroutine(HideMirror());
            }
        }
    }

    IEnumerator ShowMirror()
    {
        isAnimating = true;

        if (disablePlayerMovement)
        {
            // Zak�z�n� pohybu hr��e a kamery
            playerController.playerCanMove = false;
            playerController.cameraCanMove = false;

            // Resetov�n� prom�nn�ch pohybu, aby se zastavila animace ch�ze
            playerController.isWalking = false;
        }

        // Aktivace objektu zrcadla
        mirrorObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float curveT = movementCurve.Evaluate(t);

            // Interpolace pozice s pou�it�m k�ivky
            Vector3 currentPosition = Vector3.Lerp(mirrorHiddenPositionTransform.localPosition, mirrorHoldPositionTransform.localPosition, curveT);

            // P�id�n� t�esen�
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity * (1f - curveT);
            currentPosition += shakeOffset;

            // Interpolace z�kladn� rotace
            Quaternion baseRotation = Quaternion.Slerp(mirrorHiddenPositionTransform.localRotation, mirrorHoldPositionTransform.localRotation, curveT);

            // P�id�n� 360� rotace kolem osy Y p�i vytahov�n�
            float rotationAngle = 360f * curveT; // od 0 do 360 stup��
            Quaternion spinRotation = Quaternion.Euler(0f, rotationAngle, 0f);

            // Kombinace z�kladn� rotace a ot��en�
            Quaternion currentRotation = baseRotation * spinRotation;

            // P�id�n� drobn�ho t�esen� k rotaci
            float shakeAngle = Mathf.Sin(elapsedTime * shakeFrequency) * shakeIntensity * (1f - curveT) * 10f;
            Quaternion shakeRotation = Quaternion.Euler(shakeAngle, shakeAngle, shakeAngle);
            currentRotation *= shakeRotation;

            // Aplikace pozice a rotace
            mirrorObject.transform.localPosition = currentPosition;
            mirrorObject.transform.localRotation = currentRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ujist�te se, �e je zrcadlo p�esn� v c�lov� pozici a rotaci
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

            // Interpolace pozice s pou�it�m k�ivky (zp�t z bodu B do bodu A)
            Vector3 currentPosition = Vector3.Lerp(mirrorHoldPositionTransform.localPosition, mirrorHiddenPositionTransform.localPosition, curveT);

            // P�id�n� t�esen�
            Vector3 shakeOffset = Random.insideUnitSphere * shakeIntensity * (1f - curveT);
            currentPosition += shakeOffset;

            // Interpolace z�kladn� rotace
            Quaternion baseRotation = Quaternion.Slerp(mirrorHoldPositionTransform.localRotation, mirrorHiddenPositionTransform.localRotation, curveT);

            // P�id�n� 360� rotace kolem osy Y p�i schov�v�n�
            float rotationAngle = 360f * (1f - curveT); // od 360 do 0 stup��
            Quaternion spinRotation = Quaternion.Euler(0f, rotationAngle, 0f);

            // Kombinace z�kladn� rotace a ot��en�
            Quaternion currentRotation = baseRotation * spinRotation;

            // P�id�n� drobn�ho t�esen� k rotaci
            float shakeAngle = Mathf.Sin(elapsedTime * shakeFrequency) * shakeIntensity * (1f - curveT) * 10f;
            Quaternion shakeRotation = Quaternion.Euler(shakeAngle, shakeAngle, shakeAngle);
            currentRotation *= shakeRotation;

            // Aplikace pozice a rotace
            mirrorObject.transform.localPosition = currentPosition;
            mirrorObject.transform.localRotation = currentRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ujist�te se, �e je zrcadlo p�esn� ve skryt� pozici a rotaci
        mirrorObject.transform.localPosition = mirrorHiddenPositionTransform.localPosition;
        mirrorObject.transform.localRotation = mirrorHiddenPositionTransform.localRotation;

        // Deaktivace objektu zrcadla
        mirrorObject.SetActive(false);

        if (disablePlayerMovement)
        {
            // Povolen� pohybu hr��e a kamery
            playerController.playerCanMove = true;
            playerController.cameraCanMove = true;
        }

        isAnimating = false;
        mirrorActive = false;
    }
}