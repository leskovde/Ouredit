using System;
using System.Collections.Generic;
using System.Text;

namespace Components.Controllers
{
    public interface ICommand
    {
        string Name { get; set; }
        void Execute();
        void Undo();
    }
}
