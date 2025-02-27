using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class BaseHealth : Health
{
    async protected override void Die()
    {
        Debug.Log("Base got destroyed. Game Over!");
        await Task.Delay(500);
        Time.timeScale = 0;
        // Add respawn or game-over logic here
    }

}
