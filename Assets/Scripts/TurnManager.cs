using UnityEngine;

public class TurnManager
{
    //Callback
    public event System.Action OnTick;
    public int turnCount = 1;

    public void TurnTick()
    {
        turnCount++;
        //? means-> if ontick != null
        OnTick?.Invoke();
    }
}