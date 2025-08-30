using System;

public class StateMachine<TContext>
{
    public TContext Context { get; private set; }
    public State<TContext> CurrentState { get; private set; }
    public event Action<State<TContext>> OnStateChanged = delegate { };

    public StateMachine(TContext context)
    {
        Context = context;
    }

    public void ChangeState(State<TContext> newState)
    {
        if(CurrentState == newState) 
            return;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();

        OnStateChanged.Invoke(newState);
    }

    public void Update()
    {
        CurrentState?.Update();
    }

    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }

    public void Destroy()
    {
        CurrentState?.Exit();
        CurrentState = null;
    }
}
