using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarketEntrance : MonoBehaviour
{
    public int nextScene;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.collider.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
