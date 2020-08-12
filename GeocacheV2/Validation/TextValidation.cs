using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GeocacheV2.Validation
{
    class TextValidation : ValidationRule
    {
        public string Type { get; set; }
        const int MAX_HEADER_LENGHT = 20;
        const int MAX_CONTENT_LENGHT = 250;

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (String.IsNullOrWhiteSpace(value as string))
                return new ValidationResult(false, "Field cant be empty");
            else
                switch (Type)
                {
                    case "Header":
                        if ((value as string).Length > MAX_HEADER_LENGHT)
                            return new ValidationResult(false, 
                                String.Format("Header cant be longer than {0} characters",MAX_HEADER_LENGHT));
                        break;
                    case "Content":
                        if ((value as string).Length > MAX_CONTENT_LENGHT)
                            return new ValidationResult(false, 
                                String.Format("Content cant be longer than {0} characters",MAX_CONTENT_LENGHT));
                        break;
                }
            return new ValidationResult(true, null);
        }
    }
}
