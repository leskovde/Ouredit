namespace Components.Commands
{
    [Leskovar]
    public interface ICommand
    {
        string Name { get; set; }
        void Execute();
        void Undo();
    }
}
