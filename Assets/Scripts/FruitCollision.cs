using NUnit.Compatibility;
using UnityEngine;

public class FruitCollision : MonoBehaviour
{
    public GameObject halfFruitPrefab;
    public bool isSliced = false;
    public GameObject fruitParticle;
    public bool isBomb = false;

    private GameManager gameManager;
    [SerializeField] private AudioClip[] fruitSlashClips;
    [SerializeField] private AudioClip bombSoundClip;

    void Start()
    {
        GameObject gmObject = GameObject.Find("GameManager");

        if (gmObject != null)
        {
            gameManager = gmObject.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("GameManager not found! Check if it's in the scene and named correctly.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Debug.Log($"{gameObject.name}: Collision with terrain destroying fruit");
            Destroy(gameObject);
            if(gameObject.tag == "Fruit" && gameManager.gameMode == "Classic")
            {
                gameManager.LoseLife();
            }
        }
    }

    /// <summary>
    /// function when there collision between the katana and a fruit
    /// </summary>
    public void Slice()
    {
        if (isBomb)
        {
            PlayFX();
            gameManager.GameModeFinished(true);
            Destroy(gameObject);
            return;
        }

        if (isSliced) return; // if its sliced skip, we dont need to slice again
        isSliced = true; // mark as sliced
        
        Debug.Log($"{gameObject.name}: Sliced!");
        Debug.Log($"Game Mode: {gameManager.gameMode}");
        gameManager.AddScore();

        // saving position and rotation so we can use it while we create other halves
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        // plays the visual effect when being slashed and destroy the original
        PlayFX();
        Destroy(gameObject);

        // spawning them a little bit away from each other
        Vector3 spawnOffset = originalRotation * Vector3.right * 0.15f;

        // changin rotations so the insides look at eacht other
        Quaternion rotation1 = originalRotation * Quaternion.Euler(0, 0, 90);
        Quaternion rotation2 = originalRotation * Quaternion.Euler(0, 0, -90);

        // create new halves when being sliced with original position and rotation
        GameObject halfFruit1 = Instantiate(halfFruitPrefab, originalPosition + spawnOffset, rotation1);
        halfFruit1.GetComponent<FruitCollision>().isSliced = true;

        GameObject halfFruit2 = Instantiate(halfFruitPrefab, originalPosition - spawnOffset, rotation2);
        halfFruit2.GetComponent<FruitCollision>().isSliced = true;

        // forces applied for realistic slice
        Rigidbody rb1 = halfFruit1.GetComponent<Rigidbody>();
        Rigidbody rb2 = halfFruit2.GetComponent<Rigidbody>();

        if (rb1 != null && rb2 != null)
        {
            rb1.AddForce(originalRotation * Vector3.left * 0.2f + Vector3.down * 1.5f, ForceMode.Impulse);
            rb2.AddForce(originalRotation * Vector3.right * 0.2f + Vector3.down * 1.5f, ForceMode.Impulse);

            rb1.AddTorque(Vector3.forward * 1.5f, ForceMode.Impulse);  // small rotation while being sliced
            rb2.AddTorque(Vector3.back * 1.5f, ForceMode.Impulse);
        }
    }

    public void PlayFX()
    {
        VFX();
        if (isBomb)
        {
            SoundFXManager.instance.PlaySoundFXClip(bombSoundClip, transform, 1f);
        }
        else
        {
            SoundFXManager.instance.PlayRandomSoundFXClip(fruitSlashClips, transform, 1f);
        }

    }

    public void VFX()
    {
        GameObject particle = Instantiate(fruitParticle, transform.position, Quaternion.identity);
        Destroy(particle, particle.GetComponent<ParticleSystem>().main.duration);
    }
}