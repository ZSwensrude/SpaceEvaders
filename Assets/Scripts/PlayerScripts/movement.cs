using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls player movement
/// </summary>
public class Movement : MonoBehaviour
{

   //For future use ;)
   private PlayerInput playerInput;

   //Controls movement distance
   public float moveDistance = 2f;

   //Controls speed during movement. Used for smoothing
   //"MoveTowards" function called during update
   public float moveSpeed = 25f;

   //Public variable that store movement direction for player
   private Vector3 transVec = new Vector3(0, 0, 0);

   //Keeps track of player's current horizontal position on grid
   private int horizontal = 0;
   //Keeps track of player's current vertical position on grid
   private int vertical = 0;

   
   /// <summary>
   /// Moves player up one unit on grid if player is able
   /// </summary>
   /// <param name="context">context of button press associated with action</param>
   public void Up(InputAction.CallbackContext context)
   {

      if (context.performed)  
      {

         if (vertical < 1)
         {

            vertical++;
            transVec[1] += moveDistance;
         }

      }
   
   }

   /// <summary>
   /// Moves player down one unit on grid if player is able
   /// </summary>
   /// <param name="context">context of button press associated with action</param>
   public void Down(InputAction.CallbackContext context) 
   {

      if (context.performed)
      {

         if (vertical > -1)
         {

            vertical--;
            transVec[1] -= moveDistance;

         }

      }


   }

   /// <summary>
   /// Moves player left one unit on grid if player is able
   /// </summary>
   /// <param name="context">context of button press associated with action</param>
   public void Left(InputAction.CallbackContext context)
   {

      if (context.performed)
      {

         if (horizontal > -1)
         {

            horizontal--;
            transVec[0] -= moveDistance;

         }

      }

   
   }

   /// <summary>
   /// Moves player right one unit on grid if player is able
   /// </summary>
   /// <param name="context">context of button press associated with action</param>
   public void Right(InputAction.CallbackContext context)
   {

      if (context.performed)
      {

         if (horizontal < 1)
         {

            horizontal++;
            transVec[0] += moveDistance;

         }

      }
      

   
   }
   
   /// <summary>
   /// Update called every frame. Moves player if movement performed
   /// </summary>
   public void FixedUpdate() 
   {
      
      float step = Time.deltaTime * moveSpeed;

      transform.position = Vector3.MoveTowards(transform.position, transVec, step);

   }

}