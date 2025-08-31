[System.Serializable]
public abstract class State<TContext>
{
    private protected TContext context;
    private protected StateMachine<TContext> stateMachine;

    public void Init(TContext context, StateMachine<TContext> stateMachine)
    {
        this.context = context;
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
    public abstract void FixedUpdate();
}
