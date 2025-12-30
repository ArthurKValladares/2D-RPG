using UnityEngine;

public class StateMachine
{
    public EntityState currentState {  get; private set; }

    public void Initialize(EntityState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(EntityState newStage) {
        currentState.Exit();
        currentState = newStage;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }
}
