using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class FruitThrower : MonoBehaviour
{
    public List<GameObject> fruitPrefabs;
    public Transform throwPoint;
    public Transform player;
    public float throwForce = 6f;
    public float arcHeight = 14f;
    public float spinSpeed = 0.05f;

    private Coroutine throwFruitCoroutine;

    void Start()
    {
        StartThrowingFruit();
    }

    public void StartThrowingFruit()
    {
        if (throwFruitCoroutine == null) // Only start if not already throwing
        {
            throwFruitCoroutine = StartCoroutine(ThrowFruitRepeatedly(2f)); // Start throwing fruit with a delay
        }
    }

    // Call this to stop throwing fruit
    public void StopThrowingFruit()
    {
        if (throwFruitCoroutine != null) // Stop if it's running
        {
            StopCoroutine(throwFruitCoroutine);
            throwFruitCoroutine = null;
        }
    }

    IEnumerator ThrowFruitRepeatedly(float delay)
    {
        while (true) // Infinite loop, will continue throwing fruits
        {
            ThrowFruit();
            yield return new WaitForSeconds(Random.value + delay); // Wait for a random delay before throwing the next fruit
        }
    }

    // Throws a random fruit towards the player
    /// <summary>
    /// The math: what we do is first calculate direction and based on the direction the speed vector (V = direction * throwforce + yUp * arcHeight)
    /// </summary>
    void ThrowFruit()
    {
        // Ensure the necessary components are assigned
        if (fruitPrefabs.Count == 0 || player == null)
        {
            Debug.LogError("Fruit Prefab or Player not assigned");
            return;
        }

        // Create a random fruit object 
        GameObject fruit = Instantiate(fruitPrefabs[FruitRandomizer()], throwPoint.position, Quaternion.identity);

        // Set a target position, slightly randomizing the x value
        Vector3 targetPosition = player.position;
        targetPosition.x += Random.Range(-1f, 1f);
        targetPosition.y += 1.5f; // Aim slightly above the player
        targetPosition.z += Random.Range(-0.5f, 0.5f);

        // Calculate a simple throw direction
        Vector3 direction = (targetPosition - throwPoint.position).normalized;

        // Apply force to the fruit
        fruit.GetComponent<Rigidbody>().linearVelocity = direction * throwForce + Vector3.up * arcHeight; // Add upward force for an arc

        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
        fruit.GetComponent<Rigidbody>().AddTorque(randomTorque * spinSpeed, ForceMode.Impulse); // make it rotate mid air
    }

    int FruitRandomizer()
    {
        int number = (int)(Random.value * 10);
        return number;
    }
}
