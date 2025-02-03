using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.Mathematics;

// GameManager.cs
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    [SerializeField] private Vector2Int gridSize = new(4, 4);
    [SerializeField] private List<Sprite> symbols;

    [Header("References")]
    [SerializeField] private Transform gridParent;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI attemptsText;

    private List<Card> _cards = new();
    private List<Card> _flippedCards = new();
    private int _score;
    private int _attempts;
    private bool _isInputEnabled = true;

    private void Awake() => Instance = this;

    void Start()
    {
        GenerateGrid();
        UpdateUI();
    }

    void GenerateGrid()
    {
        // Create symbol pairs
        var symbolPairs = new List<Sprite>();
        foreach (var symbol in symbols)
        {
            symbolPairs.Add(symbol);
            symbolPairs.Add(symbol);
        }

        // Shuffle
        for (int i = 0; i < symbolPairs.Count; i++)
        {
            int randomIndex = Random.Range(i, symbolPairs.Count);
            (symbolPairs[i], symbolPairs[randomIndex]) = (symbolPairs[randomIndex], symbolPairs[i]);
        }

        // Create cards
        for (int i = 0; i < gridSize.x * gridSize.y; i++)
        {
            var card = Instantiate(cardPrefab, gridParent);
            card.Initialize(i, symbolPairs[i]);
            _cards.Add(card);
        }
    }

    public void OnCardClicked(Card card)
    {
        if (!_isInputEnabled || _flippedCards.Count >= 2) return;

        card.Flip(true);
        _flippedCards.Add(card);

        if (_flippedCards.Count == 2)
            StartCoroutine(CheckMatch());
    }

    IEnumerator CheckMatch()
    {
        _isInputEnabled = false;
        _attempts++;
        UpdateUI();

        bool isMatch = _flippedCards[0].ID == _flippedCards[1].ID;

        if (isMatch)
        {
            _score += 100;
            UpdateUI();
            yield return new WaitForSeconds(0.5f);
            foreach (var card in _flippedCards)
                card.gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            foreach (var card in _flippedCards)
                card.Flip(false);
        }

        _flippedCards.Clear();
        _isInputEnabled = true;

        CheckGameComplete();
    }

    void CheckGameComplete()
    {
        if (_cards.All(c => !c.gameObject.activeSelf))
        {
            // Show game complete screen
            UIManager.Instance.ShowGameOver(_score, _attempts);
        }
    }

    void UpdateUI()
    {
        scoreText.text = $"Score: {_score}";
        attemptsText.text = $"Attempts: {_attempts}";
    }

    // Add to GameManager.cs
    public void SetGridSize(Vector2Int newSize)
    {
        gridSize = newSize;
        ResetGame();
    }

    public void ResetGame()
    {
        foreach (var card in _cards)
            Destroy(card.gameObject);

        _cards.Clear();
        _flippedCards.Clear();
        _score = 0;
        _attempts = 0;

        GenerateGrid();
        UpdateUI();
    }
}
