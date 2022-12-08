using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class TextUI : MonoBehaviour
{
    public static TextUI instance;

    private Queue<string> textQueue = new Queue<string>();

    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private Image textBorder;

    [Header("Intro Text")]
    [SerializeField] private string introText;
    [SerializeField] private float introTextDuration;

    [Header("Boss Killed Text")]
    [SerializeField] private string bossKilledText;
    [SerializeField] private float bossKilledTextDuration;

    [Header("Player Killed Text")]
    [SerializeField] private string playerKilledText;
    [SerializeField] private float playerKilledTextDuration;

    private bool permanentTextIsDisplayed; // doors have permanent text where the text stays on screen until a condition is met (player stays in door hitbox will keep text on the screen)

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        LevelManager.instance.onAllRoomsSpawned += DisplayIntroText;
        EnemyManager.enemyManagerInstance.onBossKill += DisplayBossKilledText;

        //PlayerStats.instance.AccessPlayerHealth().onPlayerDeath += DisplayPlayerDiedText;
    }
    private void OnDisable()
    {
        LevelManager.instance.onAllRoomsSpawned -= DisplayIntroText;
        EnemyManager.enemyManagerInstance.onBossKill -= DisplayBossKilledText;
    }

    private void OnDestroy()
    {
        LevelManager.instance.onAllRoomsSpawned -= DisplayIntroText;
        EnemyManager.enemyManagerInstance.onBossKill -= DisplayBossKilledText;
    }
    public void DisplayIntroText()
    {
        TextEnqueue(introText, introTextDuration);
    }
    public void DisplayBossKilledText()
    {
        TextEnqueue(bossKilledText, bossKilledTextDuration);
    }
    public void DisplayPlayerDiedText()
    {
        TextEnqueue(playerKilledText, playerKilledTextDuration);
    }


    public void TextEnqueue(string text, float durationOfText)
    {
        
        StartCoroutine(WaitForDequeue(text, durationOfText));
    }

    IEnumerator WaitForDequeue(string text,float duration)
    {
        // don't display text until all text ahead of this text in queue has been displayed
        while (textQueue.Count != 0)
        {
            yield return null;
        }

        // enable border 
        textBorder.enabled = true;
        // add this text to the queue
        textQueue.Enqueue(text);

        // display text
        textMesh.text = text;

        // wait duration
        yield return new WaitForSeconds(duration);

        // after text displays, remove text
        textMesh.text = "";

        // disable border
        textBorder.enabled = false;

        // remove text from queue
        textQueue.Dequeue();


    }

}
