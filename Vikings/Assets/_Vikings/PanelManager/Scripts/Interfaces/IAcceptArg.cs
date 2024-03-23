namespace PanelManager.Scripts.Interfaces
{
    public interface IAcceptArg<in T>
    {
        void AcceptArg(T arg);
    }
    
    public interface IAcceptArgTwo<in TM, in TN>
    {
        void AcceptArg(TM arg1, TN arg2);
    }
}