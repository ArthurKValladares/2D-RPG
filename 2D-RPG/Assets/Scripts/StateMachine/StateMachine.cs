using UnityEngine;

public class StateMachine
{
    public EntityState currentState {  get; private set; }
    public bool canChangeState;

    public void Initialize(EntityState startState)
    {
        canChangeState = true;
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(EntityState newStage) {
        if (!canChangeState) return;

        currentState.Exit();
        currentState = newStage;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }

    public void SwitchOff()
    {
        canChangeState = false;
    }
}
