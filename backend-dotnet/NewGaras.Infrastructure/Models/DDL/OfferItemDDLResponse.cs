using NewGaras.Infrastructure.Models.DDL.UsedInResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.DDL
{
    [DataContract]
    public class OfferItemDDLResponse
    {
        List<OfferItemDDL> dDLList;
        bool result;
        List<Error> errors;
        PaginationHeader paginationHeader;

        [DataMember]
        public PaginationHeader PaginationHeader
        {
            get
            {
                return paginationHeader;
            }

            set
            {
                paginationHeader = value;
            }
        }


        [DataMember]
        public List<OfferItemDDL> DDLList
        {
            get
            {
                return dDLList;
            }

            set
            {
                dDLList = value;
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
