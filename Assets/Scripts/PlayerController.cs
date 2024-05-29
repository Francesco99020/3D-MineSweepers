using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    float movementSpeed = 5;
    float bounceHeight = 0.4f;
    float bounceTime = 0.3f;
    int currentWalkNum = 0;

    public float topBoarder;
    public float leftBoarder;
    public float rightBoarder;
    public float bottomBoarder;

    AudioSource audioSource;

    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip tileReveal;
    [SerializeField] AudioClip flagPlacement;
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip[] walkingSounds = new AudioClip[21];

    Coroutine currentWalkingBounce = null;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift)) 
        { 
            movementSpeed = 10;
            bounceTime = 0.2f;
        }
        else 
        { 
            movementSpeed = 5;
            bounceTime = 0.3f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 prevPos = transform.position;
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
            Vector3 newPos = transform.position;
            if((newPos.x > topBoarder || newPos.x < bottomBoarder) && (newPos.z > leftBoarder || newPos.z < rightBoarder)) transform.position = prevPos;
            else if (newPos.x > topBoarder || newPos.x < bottomBoarder) transform.position = new Vector3(prevPos.x, transform.position.y, newPos.z);
            else if (newPos.z > leftBoarder || newPos.z < rightBoarder) transform.position = new Vector3(newPos.x, transform.position.y, prevPos.z);
            if (currentWalkingBounce == null) currentWalkingBounce = StartCoroutine(WalkingBounce(bounceTime));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Vector3 prevPos = transform.position;
            transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
            Vector3 newPos = transform.position;
            if ((newPos.x > topBoarder || newPos.x < bottomBoarder) && (newPos.z > leftBoarder || newPos.z < rightBoarder)) transform.position = prevPos;
            else if (newPos.x > topBoarder || newPos.x < bottomBoarder) transform.position = new Vector3(prevPos.x, transform.position.y, newPos.z);
            else if (newPos.z > leftBoarder || newPos.z < rightBoarder) transform.position = new Vector3(newPos.x, transform.position.y, prevPos.z);
            if (currentWalkingBounce == null) currentWalkingBounce = StartCoroutine(WalkingBounce(bounceTime));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Vector3 prevPos = transform.position;
            transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
            Vector3 newPos = transform.position;
            if ((newPos.x > topBoarder || newPos.x < bottomBoarder) && (newPos.z > leftBoarder || newPos.z < rightBoarder)) transform.position = prevPos;
            else if (newPos.x > topBoarder || newPos.x < bottomBoarder) transform.position = new Vector3(prevPos.x, transform.position.y, newPos.z);
            else if (newPos.z > leftBoarder || newPos.z < rightBoarder) transform.position = new Vector3(newPos.x, transform.position.y, prevPos.z);
            if (currentWalkingBounce == null) currentWalkingBounce = StartCoroutine(WalkingBounce(bounceTime));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Vector3 prevPos = transform.position;
            transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
            Vector3 newPos = transform.position;
            if ((newPos.x > topBoarder || newPos.x < bottomBoarder) && (newPos.z > leftBoarder || newPos.z < rightBoarder)) transform.position = prevPos;
            else if (newPos.x > topBoarder || newPos.x < bottomBoarder) transform.position = new Vector3(prevPos.x, transform.position.y, newPos.z);
            else if (newPos.z > leftBoarder || newPos.z < rightBoarder) transform.position = new Vector3(newPos.x, transform.position.y, prevPos.z);
            if (currentWalkingBounce == null) currentWalkingBounce = StartCoroutine(WalkingBounce(bounceTime));
        }
    }

    IEnumerator WalkingBounce(float duration)
    {
        audioSource.PlayOneShot(walkingSounds[currentWalkNum], 0.1f);
        currentWalkNum++;
        if(currentWalkNum == walkingSounds.Length) { currentWalkNum = 0; }
        float start = transform.position.y;
        float end = transform.position.y + bounceHeight;
        var playerTransform = transform.position;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            playerTransform.y = Mathf.Lerp(start, end, normalizedTime);
            transform.position = new Vector3(transform.position.x, playerTransform.y, transform.position.z);
            yield return null;
        }
        playerTransform.y = end; //without this, the value will end at something like 0.9992367
        transform.position = new Vector3(transform.position.x, playerTransform.y, transform.position.z);

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
            playerTransform.y = Mathf.Lerp(end, start, normalizedTime);
            transform.position = new Vector3(transform.position.x, playerTransform.y, transform.position.z);
            yield return null;
        }
        playerTransform.y = start; //without this, the value will end at something like 0.9992367
        transform.position = new Vector3(transform.position.x, playerTransform.y, transform.position.z);
        currentWalkingBounce = null;
    }

    public void PlayExplosionAudio() { audioSource.PlayOneShot(explosion); }
    public void PlayTileRevealAudio() { audioSource.PlayOneShot(tileReveal, 0.1f); }
    public void PlayFlagPlacementAudio() { audioSource.PlayOneShot(flagPlacement, 0.1f); }
    public void PlayWinAudio() { audioSource.PlayOneShot(win); }


}