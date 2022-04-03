using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockJump : MonoBehaviour
{
    PlayerMovement4 move;

    bool hasCome;

    Game game;
    private void Awake()
    {
        game = FindObjectOfType<Game>();
        move = FindObjectOfType<PlayerMovement4>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(hasCome == false)
            {
                game.TakeOffArmor(3, true);
                move.maxJumpCounter = 2;
                hasCome = true;
            }
            
        }
    }
}
