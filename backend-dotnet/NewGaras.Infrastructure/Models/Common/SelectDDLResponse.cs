using NewGarasAPI.Models.User;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.Common
{
    public class SelectDDLResponse
    {
        public List<SelectDDL> DDLList { get; set; }
        public bool Result {  get; set; }
        public List<Error> Errors { get; set; }
    }

}
