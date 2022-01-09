public class State<T> {
    public virtual void Enter(StateMachine<T> obj) { }
    public virtual void Exit(StateMachine<T> obj) { }
    public virtual void Update(StateMachine<T> obj) { }

    public virtual void BDown(StateMachine<T> obj) {

    }

    public virtual void FixedUpdate(StateMachine<T> obj) {

    }

    public virtual string GetDebugName() {
        return "";
    }
}



