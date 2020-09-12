using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace TextEditor.Controllers
{
    public class DialogsController : Controller
    {
        public IActionResult SettingsWindow()
        {
            return View();
        }
    }
}
