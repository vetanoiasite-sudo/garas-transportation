using NewGarasAPI.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.DDL
{
    [DataContract]
    public class SellingProductsDDLResponse
    {
        List<SelectDDL> sellingProductsDDL;
        bool result;
        List<Error> errors;


        [DataMember]
        public List<SelectDDL> SellingProductsDDL
        {
            get
            {
                return sellingProductsDDL;
            }

            set
            {
                sellingProductsDDL = value;
            }
        }
        [DataMember]
        public bool Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }

        [DataMember]
        public List<Error> Errors
        {
            get
            {
                return errors;
            }

            set
            {
                errors = value;
            }
        }

    }

}
