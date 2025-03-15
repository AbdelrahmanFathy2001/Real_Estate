namespace Project.API.Helpers
{
    public class PictureUrlResolver<T, TDto> : IValueResolver<T, TDto, string> where T : IHasPictureUrl
    {
        private readonly IConfiguration _configuration;

        public PictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(T source, TDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
            {
                return string.Empty; // Return an empty string instead of null
            }

            var apiUrl = _configuration["ApiUrl"];
            if (string.IsNullOrEmpty(apiUrl))
            {
                // Optionally log an error or return a default value if ApiUrl is missing
                return string.Empty;
            }

            // Ensure the picture URL is correctly formed
            return new Uri(new Uri(apiUrl), source.PictureUrl).ToString();
        }
    }

}
