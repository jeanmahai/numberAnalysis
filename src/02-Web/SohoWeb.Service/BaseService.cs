using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SohoWeb.Service
{
    public class BaseService<T> where T : new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new T();
                return _instance;
            }
        }
    }
}
