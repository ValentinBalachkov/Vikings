namespace PanelManager.Scripts.Interfaces
{
    public interface IAcceptArg<in T>
    {
        void AcceptArg(T arg);
    }
}