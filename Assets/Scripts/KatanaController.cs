using System.Collections.Generic;
using UnityEngine;

public class KatanaController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fruit") || collision.gameObject.CompareTag("Bomb"))
        {
            collision.gameObject.GetComponent<FruitCollision>()?.Slice();
        }
    }
}
