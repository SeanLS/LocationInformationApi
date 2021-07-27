using LocationAPI.Models.Phone;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LocationAPI.Models.Phone.NumberTypeEnum;

namespace LocationAPI.Repository.Phone
{
    public class PhoneNumberMetadataRepository : IPhoneNumberMetadataRepository
    {
        private static PhoneNumberUtil PhoneNumberUtil { get; set; }

        public PhoneNumberMetadataRepository()
        {
            PhoneNumberUtil = PhoneNumberUtil.GetInstance();
        }

        public async Task<PhoneNumberMetadata> GetPhoneNumberMetadataDetails(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var metadataForRegion = await Task.Run(() => PhoneNumberUtil.GetMetadataForRegion(code));

                if (metadataForRegion == null)
                {
                    return await Task.FromResult<PhoneNumberMetadata>(null);
                }

                var phoneNumberMetadata = new PhoneNumberMetadata
                {
                    NationalPrefix = metadataForRegion.NationalPrefix,
                    InternationalPrefix = metadataForRegion.InternationalPrefix,
                    GeneralDesc = new NationalNumberPatternBase
                    {
                        NationalNumberPattern = metadataForRegion.GeneralDesc.NationalNumberPattern
                    },
                    PhoneNumberFormats = new List<PhoneNumberFormatBase>()
                };

                var phoneNumberFormatBaseList = new List<PhoneNumberFormatBase>();

                if (metadataForRegion.HasFixedLine)
                {
                    phoneNumberFormatBaseList.Add(GetPhoneNumberFormatBase(metadataForRegion.FixedLine, TypeOfNumber.FixedLine));
                }

                if (metadataForRegion.HasMobile)
                {
                    phoneNumberFormatBaseList.Add(GetPhoneNumberFormatBase(metadataForRegion.Mobile, TypeOfNumber.Mobile));
                }

                if (metadataForRegion.HasPersonalNumber)
                {
                    phoneNumberFormatBaseList.Add(GetPhoneNumberFormatBase(metadataForRegion.PersonalNumber, TypeOfNumber.PersonalNumber));
                }

                if (metadataForRegion.HasVoip)
                {
                    phoneNumberFormatBaseList.Add(GetPhoneNumberFormatBase(metadataForRegion.Voip, TypeOfNumber.Voip));
                }

                if (phoneNumberFormatBaseList.Any())
                {
                    foreach (var phoneNumberFormatBaseItem in phoneNumberFormatBaseList)
                    {
                        if (phoneNumberFormatBaseItem != null)
                        {
                            phoneNumberMetadata.PhoneNumberFormats.Add(phoneNumberFormatBaseItem);
                        }
                    }
                }

                return phoneNumberMetadata;
            }

            return await Task.FromResult<PhoneNumberMetadata>(null);
        }

        private PhoneNumberFormatBase GetPhoneNumberFormatBase(PhoneNumberDesc phoneNumberDesc, TypeOfNumber typeOfNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumberDesc.ExampleNumber) || !string.IsNullOrEmpty(phoneNumberDesc.NationalNumberPattern))
            {
                return new PhoneNumberFormatBase
                {
                    Type = Enum.GetName(typeof(TypeOfNumber), typeOfNumber),
                    ExampleNumber = phoneNumberDesc.ExampleNumber,
                    NationalNumberPattern = phoneNumberDesc.NationalNumberPattern
                };
            }

            return null;
        }
    }
}
