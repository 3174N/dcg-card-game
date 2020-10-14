using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    public static GameManager instance;

    public GameObject playerArea;
    public GameObject playerDropZone;
    public GameObject enemyArea;
    public GameObject enemyDropZone;
    
    public List<GameObject> cards = new List<GameObject>();

    #endregion

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

    public void DrawCards()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject playerCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0f, 0f, 0f), Quaternion.identity);
            playerCard.transform.SetParent(playerArea.transform, false);
            playerCard.GetComponent<Card>().player = Card.Player.Player;
        }
        
        for (int i = 0; i < 5; i++)
        {
            GameObject enemyCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0f, 0f, 0f), Quaternion.identity);
            enemyCard.transform.SetParent(enemyArea.transform, false);
            enemyCard.GetComponent<Card>().player = Card.Player.Enemy;
        }
    }
}
