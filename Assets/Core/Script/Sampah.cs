using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Sampah : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Sampah") )
        {
            //Debug.Log("Hello world");
            GameManager.GameInstance.AddScore(30f);
            GameManager.GameInstance.UpdateScore?.Invoke(GameManager.GameInstance.Score);
            GameManager.GameInstance.NextLevel();

            Destroy(collision.gameObject,0.5f);
        }

    }



}
