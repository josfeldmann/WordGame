using System;
using UnityEngine;

public class StateMachine<T> {
    public State<T> currentState;
    public T target;
    public StateMachine(State<T> startState, T obj) {
        currentState = startState;
        target = obj;
        currentState.Enter(this);
    }
    public void ChangeState(State<T> state) {
        currentState.Exit(this);
        currentState = state;
        state.Enter(this);
    }
    public void Update() {

        currentState.Update(this);
    }

    public void FixedUpdate() {
        currentState.FixedUpdate(this);
    }
}

   



