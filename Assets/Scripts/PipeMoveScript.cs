using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public float moveSpeed = 5;
    public float deadZone = -45;
    //public LogicScript logic;
    static public float maxMoveSpeed = 15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        float PS = LogicScript.Instance.playerScore;
        moveSpeed = Mathf.Clamp(5 + PS * 0.2f,0,maxMoveSpeed);
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime; 
        if (transform.position.x < deadZone)
        {
            //Debug.Log("Pipe Delete");
            Destroy(gameObject); 
        }
    }
}
