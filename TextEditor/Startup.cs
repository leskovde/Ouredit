using System;
using System.Linq;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TextEditor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            Bootstrap();
        }

        public async void Bootstrap()
        {
            var options = new BrowserWindowOptions()
            {
                Show = false
            };

           var mainWindow = await Electron.WindowManager.CreateWindowAsync(options);
           mainWindow.OnReadyToShow += () => { mainWindow.Show(); };

           var indexMenu = new MenuItem[]
           {
               new MenuItem
               {
                   Label = "File",
                   Submenu = new MenuItem[]
                   {
                       new MenuItem
                       {
                           Label = "[NI]New",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Open...",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Save",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Save as...",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Close",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Close All",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Exit",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                   }
               },
               new MenuItem
               {
                   Label = "Edit",
                   Submenu = new MenuItem[]
                   {
                       new MenuItem
                       {
                           Label = "[NI]Undo",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Redo",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Cut",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Copy",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Paste",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Delete",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Select All",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Indent",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Convert Case to",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Comment/Uncomment",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Auto-Completion",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                   }
               },
               new MenuItem
               {
                   Label = "Search",
                   Submenu = new MenuItem[]
                   {
                       new MenuItem
                       {
                           Label = "[NI]Find...",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Find in Files...",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Find Next",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Find Previous",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Replace",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Go to...",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                   }
               },
               new MenuItem
               {
                   Label = "View",
                   Submenu = new MenuItem[]
                   {
                       new MenuItem
                       {
                           Label = "[NI]Toggle Full Screen Mode",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Show Symbol",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Fold All",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]Unfold All",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                   }
               },
               new MenuItem
               {
                   Label = "Encoding",
                   Submenu = new MenuItem[]
                   {
                       new MenuItem
                       {
                           Label = "[NI]ANSI",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]UTF-8",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]UCS-2 BE",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                       new MenuItem
                       {
                           Label = "[NI]UCS-2 LE",
                           Click = async () => { await Electron.Dialog.ShowMessageBoxAsync("Mock"); }
                       },
                   }
               },
               new MenuItem
               {
                   Label = "Settings",
                   Submenu = new MenuItem[]
                   {
                       new MenuItem
                       {
                           Label = "[NI]Change Settings",
                           Click = async () =>
                           {
                               string path = $"http://localhost:{BridgeSettings.WebPort}/dialogs/settingswindow";

                               var options = new BrowserWindowOptions
                               {
                                   SkipTaskbar = true,
                               };

                               var settingsWindow = await Electron.WindowManager.CreateWindowAsync(options, path);
                               settingsWindow.RemoveMenu();
                           }
                       }
                   }
               }
           };

           Electron.Menu.SetApplicationMenu(indexMenu);
        }
    }
}
