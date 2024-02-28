using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls player movement
/// </summary>
public class Movement : MonoBehaviour
{

    //For future use ;)
    private PlayerInput playerInput;

    [SerializeField]
    private GameSettings gameSettings;

    //Controls movement distance
    public float moveDistance;
    //Controls speed during movement. Used for smoothing
    //"MoveTowards" function called during update
    public float moveSpeed;

    //Public variable that store movement direction for player
    private Vector3 transVec = new Vector3(0, 0, 0);
 
    //Keeps track of player's current horizontal position on grid
    private int horizontal = 0;
    //Keeps track of player's current vertical position on grid
    private int vertical = 0;

    private InputAction boost;

    [SerializeField]
    private ParticleSystem starParticles;
    private ParticleSystem.TrailModule starTrails;
    private ParticleSystem.MainModule starSpeed;
    private float defaultStarSpeed;

    [SerializeField]
    private GameController controller;


    void Start()
    {
        moveDistance = gameSettings.MoveDistance;
        moveSpeed = gameSettings.MoveSpeed;

        // set up particle system for handling
        starTrails = starParticles.trails;
        starTrails.enabled = false;
        starSpeed = starParticles.main;
        defaultStarSpeed = starSpeed.simulationSpeed;

        // set up input action for boost
        boost = new InputAction(
            type: InputActionType.Button,
            binding: "Keyboard/shift");

        boost.Enable();
    }


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

    private void Update()
    {
        // handle boosting in update so doesnt break if clicked more than once a frame
        if (boost.WasPressedThisFrame())
        {
            Debug.Log("boosting");
            gameSettings.AsteroidSpeed *= gameSettings.BoostSpeed;
            starTrails.enabled = true;
            starSpeed.simulationSpeed *= starSpeed.simulationSpeed * gameSettings.BoostSpeed;
            controller.ScoreMultiplier *= 2;
        }
        else if (boost.WasReleasedThisFrame())
        {
            Debug.Log("stopped boosting");
            gameSettings.AsteroidSpeed /= gameSettings.BoostSpeed;
            starTrails.enabled = false;
            starSpeed.simulationSpeed = defaultStarSpeed;
            controller.ScoreMultiplier /= 2;
        }
    }

}
