using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{


    [Header("Refrences")]
    [SerializeField] InputActionAsset input;

    private InputActionMap InputPlayer;    
    

    public Action<bool,Vector2> Click;


    void Start()
    {
        InputPlayer = input.FindActionMap("Player");
       
    }

    private void ClickOn()
    {

        Vector2 PositonMouse = InputPlayer.FindAction("MouseAxis").ReadValue<Vector2>();
        Click?.Invoke(InputPlayer.FindAction("Attack").IsPressed(), PositonMouse);
    }
    // Update is called once per frame
    void Update()
    {
        ClickOn();
    }
}
