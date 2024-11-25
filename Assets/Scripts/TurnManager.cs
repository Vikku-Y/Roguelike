using UnityEngine;

public class TurnManager
{
    //Callback
    public event System.Action OnTick;
    public int turnCount = 1;

    public void turnTick()
    {
        turnCount++;
        //? -> if ontick != null
        OnTick?.Invoke();
        Debug.Log(turnCount.ToString());
    }
}