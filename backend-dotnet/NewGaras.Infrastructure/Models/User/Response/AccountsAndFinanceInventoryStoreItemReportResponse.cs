using NewGaras.Infrastructure.Models.User.UsedInResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Models.User.Response
{
    [DataContract]
    public class AccountsAndFinanceInventoryStoreItemReportResponse
    {
        List<InventoryStoreItemItemMovementReport> inventoryStoreItemList;
        decimal noOfMonth;
        PaginationHeader paginationHeader;
        bool result;
        List<Error> errors;
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

        [DataMember]
        public List<InventoryStoreItemItemMovementReport> InventoryStoreItemList
        {
            get
            {
                return inventoryStoreItemList;
            }

            set
            {
                inventoryStoreItemList = value;
            }
        }

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
        public decimal NoOfMonth
        {
            get
            {
                return noOfMonth;
            }

            set
            {
                noOfMonth = value;
            }
        }
    }

}
