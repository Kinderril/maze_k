using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BaseWindow : MonoBehaviour
{
    public GameStage gameStage;
    public GameController gameController;

    public virtual void Init(GameController gc)
    {
        this.gameController = gc;
    }

    public virtual void Dispose()
    {
        
    }

    
}

