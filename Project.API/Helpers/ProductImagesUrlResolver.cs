using Project.BLL.Dtos.Office;

namespace Project.API.Helpers
{

    public class ProductImagesUrlResolver<T, TDto> : IValueResolver<T, TDto, List<ImageToReturnDto>> where T : IHasListImages
    {
        private readonly IConfiguration _configuration;

        public ProductImagesUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<ImageToReturnDto> Resolve(T source, TDto destination, List<ImageToReturnDto> destMember, ResolutionContext context)
        {
            if (source.Images == null || !source.Images.Any())
            {
                return new List<ImageToReturnDto>(); // Return an empty list if no images
            }

            var apiUrl = _configuration["ApiUrl"];
            if (string.IsNullOrEmpty(apiUrl))
            {
                // Optionally log a warning if ApiUrl is missing
                return new List<ImageToReturnDto>();
            }

            return source.Images.Select(img => new ImageToReturnDto
            {
                Id = img.Id,
                Name = new Uri(new Uri(apiUrl), img.PictureUrl).ToString() // Ensure valid URL formation
            }).ToList();
        }
    }


}
