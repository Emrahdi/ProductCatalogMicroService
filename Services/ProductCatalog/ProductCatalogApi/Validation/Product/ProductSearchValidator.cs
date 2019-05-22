using FluentValidation;
using ProductCatalogApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogApi.Validation.Product {
    /// <summary>
    /// Validates product searching request properties.
    /// </summary>
    public class ProductSearchValidator : AbstractValidator<ProductSearchModel> {

        /// <summary>
        /// initializes validation rules.
        /// </summary>
        public ProductSearchValidator() {
            RuleFor(x => x).NotNull().WithMessage("Product Search criteria can not  be null!");
            RuleFor(x => x.Code).NotEmpty().WithMessage("Product Code can not be empty!");
        }
    }
}