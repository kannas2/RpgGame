abstract public class FSM_State<t>
{
    abstract public void EnterState(t _monster);
    abstract public void UpdateState(t _monster);
    abstract public void ExitState(t _monster); 
}