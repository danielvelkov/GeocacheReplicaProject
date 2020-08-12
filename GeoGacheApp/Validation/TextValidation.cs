using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Geocache.Validation
{
    class TextValidation : ValidationRule
    {
        public string Type { get; set; }

        // TODO validation things
        // -if another article with the name exists
        // ...

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (String.IsNullOrWhiteSpace(value as string))
                return new ValidationResult(false, "Field cant be empty");
            else
                switch (Type)
                {
                    case "Header":
                        if ((value as string).Length > 20)
                            return new ValidationResult(false, "Header cant be longer than 20 characters");
                        break;
                    case "Comment":
                        if ((value as string).Length > 250)
                            return new ValidationResult(false, "Comment cant be longer than 250 characters");
                        break;
                }
            return new ValidationResult(true, null);
        }
    }
}
