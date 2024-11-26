using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : CellObject {

    public int food;

    public override void PlayerEntered()
    {
        //base.PlayerEntered(); ???

        Destroy(gameObject);
        GameManager.Instance.ChangeFood(food+1);
    }
}
