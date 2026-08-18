using NewGarasAPI.Models.Common;
using System.Runtime.Serialization;

namespace NewGarasAPI.Models.HR
{
    public class ContractLeaveSettingListRespoonse
    {
        List<GetContractLeaveSetting> contractLeaveSettings;
        bool result;
        List<Error> errors;
        [DataMember]
        public List<GetContractLeaveSetting> ContractLeaveSettings
        {
            get { return contractLeaveSettings; }
            set { contractLeaveSettings = value; }
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
