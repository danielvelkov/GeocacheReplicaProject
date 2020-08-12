using GeocacheV2.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GeocacheV2.Validation
{
    class LoginValidation : ValidationRule
    {
        public string Type { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (String.IsNullOrWhiteSpace(value as string))
                return new ValidationResult(false, "Field cant be empty");
            else
                switch (Type)
                {
                    case "Username":
                        if ((value as string).Length > 20)
                            return new ValidationResult(false, "Username too long");
                        else
                        {
                            using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                            {
                                if (unitOfWork.Users.DoesUserExist(value as string))
                                    break;
                                else
                                    return new ValidationResult(false, "No such user exists");
                            }
                        }
                    case "Password":
                        if ((value as string).Length > 20)
                            return new ValidationResult(false, "Password too long"); //
                        break;
                }
            return new ValidationResult(true, null);
        }
    }
}
