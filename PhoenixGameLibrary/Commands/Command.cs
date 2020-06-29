namespace PhoenixGameLibrary.Commands
{
    public abstract class Command
    {
        public object Payload { get; set; }
        internal abstract void Execute();
    }
}