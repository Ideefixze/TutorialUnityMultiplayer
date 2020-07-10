using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsView : MonoBehaviour
{
    public Text currencyText;
    public GameOperator gameOperator;

    public void Awake()
    {
        Init();
    }
    public void Init()
    {
        gameOperator.gameExecutor.onStateUpdated += UpdateView;
    }
    public void AddCoins()
    {
        var cmd = new AddPointsCommand(Random.Range(1, 5));
        gameOperator.gameExecutor.Execute(cmd);
    }
    public void RemoveCoins()
    {
        var cmd = new AddPointsCommand(-Random.Range(1, 5));
        gameOperator.gameExecutor.Execute(cmd);
    }
    public void UpdateView(GameState gameState)
    {
        currencyText.text = "Coins: " + gameState.points;
    }
    private void OnDestroy()
    {
        gameOperator.gameExecutor.onStateUpdated -= UpdateView;
    }
    
}
