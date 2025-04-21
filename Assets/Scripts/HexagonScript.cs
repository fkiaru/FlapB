using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class HexagonScript : MonoBehaviour
{
    public GameObject hexagone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(280,310, 220) * Time.deltaTime);
    }
}
