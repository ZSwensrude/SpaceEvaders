using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StarBoost : MonoBehaviour
{
    private InputAction boost;
    // Start is called before the first frame update
    void Start()
    {

        boost = new InputAction(
            type: InputActionType.Button,
            binding: "Keyboard/space");

        boost.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        if (boost.WasPressedThisFrame())
        {
            Debug.Log("boosting");
        } 
        else if (boost.WasReleasedThisFrame())
        {
            Debug.Log("stopped boosting");
        }
    }
}
