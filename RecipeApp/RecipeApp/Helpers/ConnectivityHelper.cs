using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace RecipeApp.Helpers
{
    public class ConnectivityHelper : IConnectivityHelper
    {
        public bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
    }
}
