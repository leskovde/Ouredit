using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Controllers
{
    [Leskovar]
    public interface ICommand
    {
        string Name { get; set; }
        void Execute();
        void Undo();
    }
}
