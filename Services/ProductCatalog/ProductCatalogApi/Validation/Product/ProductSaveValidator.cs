using FluentValidation;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Validation.Product {
    /// <summary>
    /// Product saving validation class
    /// </summary>
    public class ProductSaveValidator : AbstractValidator<ProductModel> {
        /// <summary>
        /// initializes validations
        /// </summary>
        public ProductSaveValidator() {
            RuleFor(x => x).NotNull().WithMessage("Product can not  be null!");
            RuleFor(x => x.Code).NotEmpty().WithMessage("Product Code can not be empty!");
            RuleFor(x => x.Price).ExclusiveBetween(0, 999).WithMessage("Price should be between 0 and 999!");
        }
    }
}
