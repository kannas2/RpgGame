abstract public class FSM_State<t>
{
    abstract public void EnterState(t monster);
    abstract public void UpdateState(t monster);
    abstract public void ExitState(t monster); 
}