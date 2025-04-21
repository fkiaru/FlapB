using UnityEngine;

public class PipeMiddleScript : MonoBehaviour
{
    //public LogicScript logic;
    public GameObject hexagone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            LogicScript.Instance.addScore(1);
            Destroy(gameObject);
        }
        


    }
}
