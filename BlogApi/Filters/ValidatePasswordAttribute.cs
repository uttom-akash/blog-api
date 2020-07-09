using System;
using System.ComponentModel.DataAnnotations;

namespace Blog_Rest_Api.Custom_Attribute
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class ValidatePasswordAttribute : ValidationAttribute
    {
        public int minAlphabet;
        public int minNumeric;
        public int minLength;
        public ValidatePasswordAttribute()
        {
            minAlphabet=1;
            minNumeric=1;
            minLength=5;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string Value=value.ToString();
            int alphabet=0,numeric=0,length=Value.Length;
            
            if(length<minLength)
                return new ValidationResult($"Password length must be {minLength}");
            
            foreach (var item in Value)
            { 
                if(char.IsLetter(item))
                    alphabet++;
                else if(char.IsDigit('1'))
                    numeric++;
            }
            if(alphabet<minAlphabet)
                return new ValidationResult($"Aphabet in Password length must be {minAlphabet}");

            if(numeric<minNumeric)
                return new ValidationResult($"Number in Password length must be {minNumeric}");
            
            return ValidationResult.Success;
        }
    }
}
