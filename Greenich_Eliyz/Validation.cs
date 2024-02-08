using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Greenich_Eliyz
{
    public class ValidName : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string name = value.ToString();
                if (name.Length < Min) // name too short
                    return new ValidationResult(false, "Too short");
                if (name.Length > Max) // name too long
                    return new ValidationResult(false, "Too long");
                foreach (char c in name)
                    if (!Char.IsLetter(c) && c != ' ')
                        return new ValidationResult(false, "Only letters and spaces");
                if (!Char.IsUpper(name[0]))
                    return new ValidationResult(false, "Name must start with capital letter");
                if (!Char.IsLetter(name[name.Length - 1]))
                    return new ValidationResult(false, "Name can't end with space");
                if (name.IndexOf("  ") != -1)
                    return new ValidationResult(false, "First name must not include more than one space at a time");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Error: " + ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }


    public class ValidUsername : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string username = value.ToString();
                if (username.Length < 6) // username too short
                    return new ValidationResult(false, "Too short");
                if (username.Length > 15) // username too long
                    return new ValidationResult(false, "Too long");
                if (!username.Any(char.IsDigit)) // must contain at least one number
                    return new ValidationResult(false, "Must contain at least one number");
                foreach (char c in username)
                    if (!Char.IsLetterOrDigit(c) && c != '_')
                        return new ValidationResult(false, "Only letters, numbers, and underscore");
                if (username.Any(char.IsUpper)) // cannot contain capital letters
                    return new ValidationResult(false, "Cannot contain capital letters");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Username not valid: " + ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }

    public class ValidPassword : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string password = value.ToString();
                string symbols = "_@#$%&~-?!";
                bool lower = false, upper = false, digit = false, sym = false;
                if (password.Length < 6) // password too short
                    return new ValidationResult(false, "pasword too short");
                if (password.Length > 16) // password too long
                    return new ValidationResult(false, "password too long");
                for (int i = 0; i < password.Length; i++)
                {
                    if (!Char.IsLetterOrDigit(password[i]) && symbols.IndexOf(password[i]) == -1)
                        return new ValidationResult(false, "Password must contain letters, digits and " + symbols);
                    if (char.IsUpper(password[i])) upper = true;
                    if (char.IsLower(password[i])) lower = true;
                    if (char.IsDigit(password[i])) digit = true;
                    if (symbols.IndexOf(password[i]) != -1) sym = true;
                }
                if (!(upper && lower && digit && sym))
                    throw new Exception("Password must contain atleast one capital letter, one lower letter, a number and a symbol");

            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "" + ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }

    public class ValidEmail : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string email = value.ToString();
                // Use a regular expression to validate the email format
                string emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
                Regex regex = new Regex(emailPattern);
                if (!regex.IsMatch(email))
                    return new ValidationResult(false, "Invalid email format");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Email not valid: " + ex.Message);
            }

            return ValidationResult.ValidResult;
        }
    }


}
