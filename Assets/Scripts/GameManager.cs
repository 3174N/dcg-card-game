using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
