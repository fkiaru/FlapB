using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PipeSpawnerScript : MonoBehaviour
{
    public GameObject pipe;
    public float spawnRate = 10f;
    public float spawnRateMin = 1.7f;
    private float timer = 0;
    public float heightOffSet = 3f;
    public float heightOffSetMax = 6.0f;
    public float rateHeightOffset = 4.0f;
    private float lastY;
    [SerializeField]
    private float maxDeltaHeight = 5.0f;
    //public LogicScript llogic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //llogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        spawnPipe();
        lastY = transform.position.y;  
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {            
            timer = timer + Time.deltaTime;
        }
        else
        {
            spawnPipe();
            timer = 0;
        }            
    }
    void spawnPipe()
    {
        // new Pipe generation        
        float lowestPoint = - heightOffSet;
        float highestPoint = + heightOffSet;
        float newY = Random.Range(lowestPoint, highestPoint);
        newY = Mathf.Clamp(newY, lastY - maxDeltaHeight, lastY + maxDeltaHeight); // evite les écarts trop grands.
        lastY = newY;
        float PS = LogicScript.Instance.playerScore;
        heightOffSet = Mathf.Clamp(3f + (PS / rateHeightOffset), 0f, heightOffSetMax); // update heightOffset

        spawnRate = Mathf.Clamp(5 - (PS / rateHeightOffset), spawnRateMin, 10f);

        //Debug.Log("Sp = "+PS.ToString()+" HOff = "+heightOffSet.ToString()+ " Spr = "+spawnRate.ToString());

        Instantiate(pipe, new Vector3(transform.position.x, newY,0), transform.rotation);

        if (PS> 30) { // HEAVY MODE
            maxDeltaHeight =6.0f;
            PipeMoveScript.maxMoveSpeed = 20f;
            spawnRateMin = 0.8f;
        }
        if (PS  > 50)
        { // HARD MODE
            maxDeltaHeight = 7.0f;
            PipeMoveScript.maxMoveSpeed = 22f;
            spawnRateMin = 0.7f;
        }
        if (PS > 80)
        { // GOD
            maxDeltaHeight = 8.0f;
            PipeMoveScript.maxMoveSpeed = 30f;
            spawnRateMin = 0.5f;
        }
    }
}
