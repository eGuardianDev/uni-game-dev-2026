using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayHealthTest
{
   
    [UnityTest]
    public IEnumerator PlayHealthTestWithEnumeratorPasses()
    {

        GameObject PlayerWithHealth = new GameObject();

        var player = PlayerWithHealth.AddComponent<Health>();

        player.currentHealth = 3;
        player.GetHit();

        // Debug.Log(player.currentHealth);
        // Debug.Log(player.can_be_hit);
        Assert.AreEqual(player.currentHealth, 2);
        yield return null;
    }

     [UnityTest]
    public IEnumerator PlayerJumpsOnPowerUp()
    {

        GameObject PlayerToJump = new GameObject();
        PlayerToJump.layer = LayerMask.NameToLayer("Player");
        
        var rb = PlayerToJump.AddComponent<Rigidbody2D>();
        var playerCollider = PlayerToJump.AddComponent<BoxCollider2D>();
        
        
        GameObject jumpingPowerUp = new GameObject();
        var powerup = jumpingPowerUp.AddComponent<PowerUp>();    
        powerup.jumpingForce = 200f;
        var trigger = jumpingPowerUp.AddComponent<BoxCollider2D>();
        trigger.isTrigger = true;


        PlayerToJump.transform.position = Vector2.zero;
        jumpingPowerUp.transform.position = Vector2.zero;

        yield return new WaitForFixedUpdate(); 
    
        Assert.Greater(rb.linearVelocity.y, 0f);
    }
}
