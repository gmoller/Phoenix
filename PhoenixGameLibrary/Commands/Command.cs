namespace PhoenixGameLibrary.Commands
{
    public abstract class Command
    {
        public object Payload { get; set; }
        public abstract void Execute();
    }
}