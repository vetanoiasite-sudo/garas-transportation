using Microsoft.EntityFrameworkCore;
using NewGaras.Infrastructure.DBContext;
using NewGaras.Infrastructure.Entities;

namespace NewGarasAPI.Helper
{
    public class GarasContext
    {
        private static GarasTestContext _instance = null;
        private GarasContext() 
        {
            _instance = new GarasTestContext();
        }
        public static GarasTestContext getInstance() 
        {
            if( _instance == null)
            {
                _instance = new GarasTestContext();
            }
            return _instance;
        }
        

    }
}
