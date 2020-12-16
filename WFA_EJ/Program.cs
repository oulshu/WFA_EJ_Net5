using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using WFA_EJ.Data;

namespace WFA_EJ
{
    public static class Program
    {
        #region Свойства

        public static ApplicationContext Context { get; set; }
        public static DataBase DataBase { get; set; }
        public static IConfigurationRoot cfg { get; set; }

        #endregion

        #region Методы

        /// <summary>
        ///     Главная точка входа для приложения.
        /// </summary>
        [STAThread] private static void Main()
        {
            var setti = new FileInfo("WFA_EJ.Settings.xml");
            if (!setti.Exists)
            {
                using var fs = setti.OpenWrite();
                var info = new UTF8Encoding(true).GetBytes(
                    "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<settings>\r\n  <password>0000</password>\r\n  <SaveNameFile>WFA_EJ.DataBase</SaveNameFile>\r\n  <SaveTypeFile>XML</SaveTypeFile>\r\n</settings>");

                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            cfg = new ConfigurationBuilder().AddXmlFile("WFA_EJ.Settings.xml").Build();
            DataBase = new DataBase();
            DataBase.Load();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if (!DEBUG)
            Context = new ApplicationContext(new F_Auth());
#endif
#if DEBUG
            Context = new ApplicationContext(new F_Main());
#endif
            Application.Run(Context);
        }

        #endregion
    }
}