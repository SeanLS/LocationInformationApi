using System.Collections.Generic;

namespace LocationAPI.Models.Phone
{
    public class PhoneNumberMetadata
    {
        public string InternationalPrefix { get; set; }

        public string NationalPrefix { get; set; }

        public NationalNumberPatternBase GeneralDesc { get; set; }

        public IList<PhoneNumberFormatBase> PhoneNumberFormats { get; set; }
    }
}
