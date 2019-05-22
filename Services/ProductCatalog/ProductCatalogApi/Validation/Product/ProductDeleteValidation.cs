using FluentValidation;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Validation.Product
{
    /// <summary>
    /// Validates product deletion request properties.
    /// </summary>
    public class ProductDeleteValidation : AbstractValidator<ProductDeleteRequestModel>
    {
        /// <summary>
        /// initializes validations
        /// </summary>
        public ProductDeleteValidation()
        {
            RuleFor(x => x).NotNull().WithMessage("Product Info can not  be null!");
            RuleFor(x => x.Code).NotNull().NotEmpty().WithMessage("Product Code can not be empty!");
        }
    }
}
