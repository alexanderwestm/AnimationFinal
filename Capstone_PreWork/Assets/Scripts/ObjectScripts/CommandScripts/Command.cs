using UnityEngine;

public abstract class Command
{
    public GameObject gameObject { get; set; }
    public float timeRan { get; set; }

    public Command(GameObject obj, float timeRan)
    {
        gameObject = obj;
        this.timeRan = timeRan;
    }

    public abstract void Execute();
    public abstract void Undo();

    public virtual bool Cancel() { return false; }

    public virtual void Init(GameObject obj)
    {
        gameObject = obj;
    }
}