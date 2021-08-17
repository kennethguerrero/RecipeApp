using RecipeApp.Helpers;
using System;
using System.Diagnostics;

namespace RecipeApp.Managers
{
    public class BaseManager
    {
        private readonly IConnectivityHelper _connectivityHelper;
        public BaseManager(IConnectivityHelper connectivityHelper)
        {
            _connectivityHelper = connectivityHelper;
        }
        public bool CheckInternetConnection()
        {
            try
            {
                return _connectivityHelper.IsConnected;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }
    }
}
