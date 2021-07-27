namespace LocationAPI.Models.PostCode
{
    public class PostalCodeDetail : LocationNameIsoDetail
    {
        public string Note { get; set; }

        public string Format { get; set; }

        public string Regex { get; set; }
    }
}
