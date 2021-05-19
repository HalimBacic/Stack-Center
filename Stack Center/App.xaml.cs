using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Stack_Center
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static App Instance;
        public static string Directory;
        public event EventHandler LanguageChangedEvent;

        private App()
        {
            Instance = this;
            Directory = Directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            SetLanguageResourceDictionary(GetLocXAMLFilePath(CultureInfo.CurrentCulture.Name));
        }

        public void ChangeTheme(Uri uri)
        {
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private string GetLocXAMLFilePath(string culture)
        {
            return "./lang/lang."+culture+".xaml";
        }

        private void SetLanguageResourceDictionary(string p)
        {
            string[] culture = p.Split(".");
            ResourceDictionary dict = new ResourceDictionary();
            switch (culture[1])
            {
                case "en-US":
                    dict.Source = new Uri("..\\lang\\lang.en-US.xaml",
                                  UriKind.Relative);
                    break;
                case "sr":
                    dict.Source = new Uri("..\\lang\\lang.sr.xaml",
                                       UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\lang\\lang.en-US.xaml",
                                      UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }

        public void LanguageChange(String cu)
        {
            LanguageChangedEvent = null;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cu);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cu);

            SetLanguageResourceDictionary(GetLocXAMLFilePath(cu));
            if (null != LanguageChangedEvent)
            {
                LanguageChangedEvent(this, new EventArgs());
            }
        }
    }
}