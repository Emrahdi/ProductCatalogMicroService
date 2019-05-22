using FluentValidation;
using ProductCatalogApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Validation.Product
{
    /// <summary>
    /// Validates image saving request properties.
    /// </summary>
    public class ProductPhotoValidator : AbstractValidator<ProductPhotoModel>
    {
        /// <summary>
        /// Initializes validations
        /// </summary>
        public ProductPhotoValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Product Info can not  be null!");
            RuleFor(x => x.ProductCode).NotNull().NotEmpty().WithMessage("Product Code can not be empty!");
            RuleFor(x => x.PhotoStringFormat).NotNull().NotEmpty().WithMessage("Product Photo can not be empty!");
        }
    }
}
