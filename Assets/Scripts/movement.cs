using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

   public float moveDistance = 2;
   public int gridUnitLength = 2;
   public int moveSpeed = 25;

   private int horizontal;
   private int vertical;
   public void Up(InputAction.CallbackContext context)
   {
      if (context.started)
      {

         if (vertical < 1)
         {

            vertical++;
            Vector3 transVec = new Vector3 (0, moveDistance, 0);
            float step = Time.deltaTime * moveSpeed;

            transform.position = Vector3.MoveTowards(transform.position, transVec, step);
         }

      }
   
   }

   public void Down(InputAction.CallbackContext context) 
   {
      if (context.started)
      {

         if (vertical > -1)
         {

            vertical--;
            Vector3 transVec = new Vector3 (0, -moveDistance, 0);
            float step = Time.deltaTime * moveSpeed;

            transform.position = Vector3.MoveTowards(transform.position, transVec, step);
         }

      }
   }

   public void Left(InputAction.CallbackContext context)
   {
      if (context.started)
      {

         if (horizontal > -1)
         {

            horizontal++;
            Vector3 transVec = new Vector3 (-moveDistance, 0, 0);
            float step = Time.deltaTime * moveSpeed;

            transform.position = Vector3.MoveTowards(transform.position, transVec, step);
         }

      }
   
   }

   public void Right(InputAction.CallbackContext context)
   {
      if (context.started)
      {

         if (horizontal < 1)
         {

            horizontal++;
            Vector3 transVec = new Vector3 (moveDistance, 0, 0);
            float step = Time.deltaTime * moveSpeed;

            transform.position = Vector3.MoveTowards(transform.position, transVec, step);
         }

      }
   
   }

}
