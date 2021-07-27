namespace LocationAPI.Models.Phone
{
    public class PhoneNumberFormatBase : NationalNumberPatternBase
    {
        public string Type { get; set; }

        public string ExampleNumber { get; set; }
    }
}
