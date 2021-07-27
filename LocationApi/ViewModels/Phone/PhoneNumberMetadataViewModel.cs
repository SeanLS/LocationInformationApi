using System.Collections.Generic;

namespace LocationAPI.Models.Phone
{
    public class PhoneNumberMetadataViewModel
    {
        public string InternationalPrefix { get; set; }

        public string NationalPrefix { get; set; }

        public NationalNumberPatternBaseViewModel GeneralDesc { get; set; }

        public IList<PhoneNumberFormatBaseViewModel> PhoneNumberFormats { get; set; }
    }
}
