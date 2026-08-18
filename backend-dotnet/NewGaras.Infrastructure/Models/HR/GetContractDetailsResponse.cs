using NewGarasAPI.Models.Common;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class GetContractDetailsResponse
    {
        ContractDetailModel contract;
        bool result;
        List<Error> errors;
        [DataMember]
        public ContractDetailModel Contract
        {
            get { return contract; }
            set { contract = value; }
        }
        [DataMember]
        public bool Result
        {
            set { result = value; }
            get { return result; }
        }
        [DataMember]
        public List<Error> Errors
        {
            set { errors = value; }
            get { return errors; }
        }
    }
}
