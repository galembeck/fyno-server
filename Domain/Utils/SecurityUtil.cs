using Domain.Enumerators;

namespace Domain.Utils;

public class SecurityUtil
{
    public static bool GetPasswordStrength(string password)
    {
        int score = 0;
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password.Trim()))
            return false;

        if (HasMinimumLength(password, 5))
            score++;

        if (HasMinimumLength(password, 8))
            score++;

        if (HasUpperCaseLetter(password) && HasLowerCaseLetter(password))
            score++;

        if (HasDigit(password))
            score++;

        if (HasSpecialChar(password))
            score++;

        if ((PasswordStrength)score != PasswordStrength.VeryStrong)
            return false;

        return true;
    }

    public static Boolean IsValidCellphone(string cellphone)
    {
        if (String.IsNullOrEmpty(cellphone))
            return false;

        Boolean response = true;
        for (int i = 0; i < cellphone.Count() - 1; i++)
        {
            var count = 0;
            for (int j = 0; j < cellphone.Count(); j++) 
            { 
                if (cellphone[i] == cellphone[j])
                {
                    count++;
                }
            }

            if (count > 8)
            {
                response = false;
                break;
            }
        }

        return response;
    }

    #region HelperMethods

    /// <summary>
    /// Returns TRUE if the password has the minimum length
    /// </summary>
    public static bool HasMinimumLength(string password, int minLength)
    {
        return password.Length >= minLength;
    }

    /// <summary>
    /// Returns TRUE if the password has the minimum number of unique characters
    /// </summary>
    public static bool HasMinimumUniqueChars(string password, int minUniqueChars)
    {
        return password.Distinct().Count() >= minUniqueChars;
    }

    /// <summary>
    /// Returns TRUE if the password has at least one digit
    /// </summary>
    public static bool HasDigit(string password)
    {
        return password.Any(c => char.IsDigit(c));
    }

    /// <summary>
    /// Returns TRUE if the password has at least one special character
    /// </summary>
    public static bool HasSpecialChar(string password)
    {
        return RegexUtil.HasSpecialCharacters(password);
    }

    /// <summary>
    /// Returns TRUE if the password has at least one uppercase letter
    /// </summary>
    public static bool HasUpperCaseLetter(string password)
    {
        return password.Any(c => char.IsUpper(c));
    }

    /// <summary>
    /// Returns TRUE if the password has at least one lowercase letter
    /// </summary>
    public static bool HasLowerCaseLetter(string password)
    {
        return password.Any(c => char.IsLower(c));
    }

    #endregion HelperMethods
}
