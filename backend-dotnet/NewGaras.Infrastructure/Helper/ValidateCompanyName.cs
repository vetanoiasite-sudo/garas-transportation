using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Helper
{
    public class ValidateCompanyName
    {
        public List<string> validStrings = new List<string> { "marinaplt", "proauto", "piaroma", "elsalam", "garastest", "elwaseem", "marinapltq", "vetanoia", "ramsissteel", "periti", "ortho", "libmark", "coctail", "royaltent", "eldib", "stmark", "shi", "shc", "stmg", "elite", "graffiti", "libroyes", "sanpeter","stmary","pharma" };
        public bool ValidateInput(string userInput)
        {
            return validStrings.Select(s => s.ToLower()).Contains(userInput.Trim().ToLower());
        }
    }
}
