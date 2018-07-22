using UnityEngine;
using System.Collections;

public class GuideHandler {

    public delegate void FirstEnterGameEvent();
    public delegate void FirstOpenBagEvent();

    public event FirstEnterGameEvent OnFirstEnterGame;
    public event FirstOpenBagEvent OnFirstOpenBag;
}
