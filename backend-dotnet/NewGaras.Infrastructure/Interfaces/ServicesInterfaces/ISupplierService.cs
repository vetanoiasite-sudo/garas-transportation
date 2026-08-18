using NewGaras.Infrastructure.Models.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGaras.Infrastructure.Interfaces.ServicesInterfaces
{
    public interface ISupplierService
    {
        public HearderVaidatorOutput Validation { get; set; }

        public BaseResponseWithId<long> AddNewSupplier(SupplierData Request);

        public Task<GetSupplierData> GetSupplierDataResponse([FromHeader] long SupplierId);

        public SuppliersCardsResponse GetSuppliersCards(GetSuppliersCardsFilters filters);
        public GetSupplierContactPersonsData GetSupplierContactPersonsResponse([FromHeader] long? SupplierId);

        public CheckSupplierExistanceResponse CheckSupplierExistance(CheckSupplierExistanceFilters filters);


        public BaseResponseWithId<long> AddSupplierAttachments([FromForm] SupplierAttachmentsData Request);

        public Task<BaseResponseWithId<long>> AddSupplierTaxCard(SupplierTaxCardData Request);

        public BaseResponseWithId<long> AddNewSupplierContacts(SupplierContactsData Request);

        public BaseResponseWithId<long> AddNewSupplierContactPerson(SupplierContactPersonData Request);

        public BaseResponseWithId<long> AddNewSupplierMobile(SupplierMobileData Request);

        public BaseResponseWithId<long> AddNewSupplierSpeciality(SupplierSpecialityData Request);

        public BaseResponseWithId<long> AddNewSupplierLandLine(SupplierLandLineData Request);
        public BaseResponseWithId<long> AddNewSupplierFax(SupplierFaxData Request);

        public SelectDDLResponse GetSupplierList([FromHeader] int GovernorateID , [FromHeader] string Import , [FromHeader] string SearchKey );
    }
}
