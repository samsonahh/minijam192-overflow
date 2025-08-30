[System.Serializable]
public abstract class State<TContext>
{
    public TContext Context { get; private set; }
    public StateMachine<TContext> StateMachine { get; private set; }

    public void Init(TContext context, StateMachine<TContext> stateMachine)
    {
        Context = context;
        StateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
    public abstract void FixedUpdate();
}
