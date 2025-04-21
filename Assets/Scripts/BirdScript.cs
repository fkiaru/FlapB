using System;
//using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;


public class BirdScript : MonoBehaviour
{
    
    public Rigidbody2D rb;
    public float flapStrength=15;
    //public LogicScript logic;  
    
    
    [Header("Parameters of animation"), Tooltip("Durée de l'animation lorsqu'il avale une pièce")]
    public float timerAvaleMax = 0.5f;
    [Tooltip("Taille maximale lorsqu'il avale"),Range(0f,2f)]
    public float SizeAvaleMax = 0.4f;
    // champs Privés
    private float timerAvale; // delai pour l'animation
    private bool isAlive = true;
    private PlayerInput playerInput;
    private InputAction touchPressAction;
    private int state = 0; // 1 : vient d'avaler

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPressAction = playerInput.actions["Space"];
    }
    public bool getAlive()
    {
        return isAlive;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        isAlive = true;
    }
    public void avale()
    {
        // déclenche l'animation avale
        state = 1;
        timerAvale = timerAvaleMax; // remise à zéro
    }
    private void Update()
    {
        if (state == 1)
        {
            // animation avale en cours
            timerAvale -= Time.deltaTime;
            if (timerAvale < 0)
            {
                state = 0;
                gameObject.transform.localScale = Vector3.one; // reinit size

            }
            else
            {
                float x = timerAvale / timerAvaleMax;
                x = 1 + x * (1 - x) * SizeAvaleMax;
                Vector3 v = new Vector3(x, x, x);
                gameObject.transform.localScale = v;
            }
        }

    }
    public void setActive(bool active)
    {
        gameObject.SetActive(active);        
    }
    public void OnEnable()
    {
        touchPressAction.performed += TouchPressed;
    }
    public void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
    }
    private void TouchPressed(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        //Debug.Log(" val KP = " + value.ToString());
        if (isAlive && !LogicScript.Instance.isPaused)
        {
            rb.linearVelocity = Vector2.up * flapStrength;
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Transform child1 = transform.Find("Queue1");
        Transform child2 = transform.Find("Queue2");
        // update the queue animation
        bool cState = rb.linearVelocityY < 0.0f;
        child1.gameObject.SetActive(!cState);
        child2.gameObject.SetActive(cState);
        
        //Renderer rr = gameObject.GetComponentsInChildren(UnityTypes.Re);
        //if (touchPressAction.IsPressed() && isAlive)
        //{
        //    rb.linearVelocity = Vector2.up * flapStrength;
        //}
        if (gameObject.transform.position.y > 16 || gameObject.transform.position.y < -16)
        {
            LogicScript.Instance.gameOver();
            isAlive = false;
        }
          
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!LogicScript.Instance.invincible)
        {
            LogicScript.Instance.gameOver();
            isAlive = false;
        }
    }
}
