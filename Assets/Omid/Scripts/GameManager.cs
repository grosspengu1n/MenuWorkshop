using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isStore;
    public bool sweetToggle;
    public bool saltyToggle;
    public bool alcoholicToggle;
    public GameObject interactive;

    public GameObject blackPanel;
    public Text dayText;
    public float fadeDuration = 1f;
    private CanvasGroup canvasGroup;
    private int currentDay = 1;
    private bool isFading = false;



    public GameObject[] itemObjects; 
    public Text[] priceTexts;

    public Sprite[] sweetSprites;
    public float[] sweetPrices; 

    public Sprite[] saltySprites;
    public float[] saltyPrices;

    public Sprite[] alcoholicSprites;
    public float[] alcoholicPrices;

    private List<int> usedIndices = new List<int>();

    public static string currentItem;
    public static int currentPrice;
    public static bool sleep;

    private void Start()
    {
        blackPanel.SetActive(true);
        canvasGroup = blackPanel.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RefreshShop();
        }
        if (PlayerController.canTalk || sleep)
        {
            interactive.SetActive(true);
        }
        else
        {
            interactive.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.E) && sleep && !isFading)
        {
            StartCoroutine(ChangeDay());
        }

    }


    public void RefreshShop()
    {
        ClearItems();
        usedIndices.Clear();

        if (sweetToggle)
        {
            SetRandomItems(sweetSprites, sweetPrices);
        }
        else if (saltyToggle)
        {
            SetRandomItems(saltySprites, saltyPrices);
        }
        else if (alcoholicToggle)
        {
            SetRandomItems(alcoholicSprites, alcoholicPrices);
        }
    }

    private void ClearItems()
    {
        foreach (var obj in itemObjects)
        {
            obj.GetComponent<SpriteRenderer>().sprite = null;
        }

        foreach (var text in priceTexts)
        {
            text.text = "";
        }
    }

    private void SetRandomItems(Sprite[] sprites, float[] prices)
    {
        for (int i = 0; i < itemObjects.Length; i++)
        {
            int randomIndex = GetUniqueRandomIndex(sprites.Length);
            usedIndices.Add(randomIndex);

            itemObjects[i].GetComponent<SpriteRenderer>().sprite = sprites[randomIndex];
            priceTexts[i].text = "$" + prices[randomIndex].ToString();
        }
    }

    private int GetUniqueRandomIndex(int maxValue)
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, maxValue);
        } while (usedIndices.Contains(randomIndex));

        return randomIndex;
    }

    private IEnumerator ChangeDay()
    {
        isFading = true;

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
            Debug.Log(Time.deltaTime / fadeDuration);
        }


        yield return new WaitForSeconds(1f);

        currentDay++;
        dayText.text = "Day " + currentDay;

        yield return new WaitForSeconds(1f);


        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
            Debug.Log(Time.deltaTime / fadeDuration);
        }

        isFading = false;
    }
}
