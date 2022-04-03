using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    Game game;

    private void Awake()
    {
        game = FindObjectOfType<Game>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            game.GameOver();
        }
    }
}