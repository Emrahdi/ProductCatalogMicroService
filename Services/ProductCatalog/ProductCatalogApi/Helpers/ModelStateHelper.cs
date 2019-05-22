using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogApi.Helpers {
    /// <summary>
    /// Mvc model state method helper
    /// </summary>
    public static class ModelStateHelper {
        /// <summary>
        /// Gets first error message from model state.
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="errorMessage"></param>
        /// <returns>true if error is taken from model state, false othervise</returns>
        public static bool TryGetFirstValidationError(ModelStateDictionary modelState, out string errorMessage) {
            errorMessage = string.Empty;
            if (modelState.IsValid) {
                return false;
            }
            var firstError = modelState.Values.SelectMany(v => v.Errors).SingleOrDefault();
            errorMessage = firstError.ErrorMessage;
            return true;
        }
        /// <summary>
        /// Gets all errors from model state
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="errorMessages"></param>
        /// <returns></returns>
        public static bool TryGetValidationErrors(ModelStateDictionary modelState, out List<string> errorMessages) {
            errorMessages = new List<string>();
            if (modelState.IsValid) {
                return false;
            }
            IEnumerable<ModelError> allErrors = modelState.Values.SelectMany(v => v.Errors);
            foreach (var item in allErrors) {
                errorMessages.Add(item.ErrorMessage);
            }
            return true;
        }


        /// <summary>
        /// Gets all errors and concatanes from model state.
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static bool TryGetValidationErrorsConcataneted(ModelStateDictionary modelState, out string err) {
            err = string.Empty;
            StringBuilder stringBuilder = new StringBuilder();
            List<string> errorMessageList = new List<string>();
            bool result = ModelStateHelper.TryGetValidationErrors(modelState, out errorMessageList);
            if (!result) {
                return false;
            }
            foreach (var errorMessage in errorMessageList) {
                stringBuilder.AppendFormat("ErrorMessage:{0},", errorMessage);
            }
            err = stringBuilder.ToString();
            return true;
        }
    }
}
