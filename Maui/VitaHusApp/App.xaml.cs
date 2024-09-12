
﻿using VitaHusApp.Pages;

namespace VitaHusApp

{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            MainPage = new VideoMenu();

            

        }
    }
}
