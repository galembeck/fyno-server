using Domain.Constants;

namespace Domain.Utils;

public static class StringUtil
{
    public static string DATEFORMAT_NUMBERS_ONLY = "yyyyMMddHHmmss";

    public static string GetDateAsNumbersOnly(DateTimeOffset? date = null)
    {
        return date == null ? DateTimeOffset.UtcNow.ToString(DATEFORMAT_NUMBERS_ONLY)
            : date.Value.ToString(DATEFORMAT_NUMBERS_ONLY);
    }

    /// <summary>
    /// Format a Date using a string format or one of the <see cref="DateFormatConstants"/> provided. Will use <see cref="DateFormatConstants.ISO_8601"/> if none are provided.
    /// </summary>
    /// <param name="format">The format to be used, can be a custom format or one of the <see cref="DateFormatConstants"/>, <see cref="DateFormatConstants.ISO_8601"/> if null.</param>
    /// <param name="date">The date to be formated, <see cref="DateTimeOffset.UtcNow"/> if null.</param>
    /// <returns></returns>
    public static string GetDateFormated(string format = null, DateTimeOffset? date = null)
    {
        format ??= DateFormatConstants.ISO_8601;

        return date == null ? DateTimeOffset.UtcNow.ToString(format)
            : date.Value.ToString(format);
    }

    public static string GetMonthInPortugueseByNumber(int month)
    {
        #pragma warning disable CS8603 // Possible null reference return.
        return month switch
        {
            1 => "Janeiro",
            2 => "Fevereiro",
            3 => "Março",
            4 => "Abril",
            5 => "Maio",
            6 => "Junho",
            7 => "Julho",
            8 => "Agosto",
            9 => "Setembro",
            10 => "Outubro",
            11 => "Novembro",
            12 => "Dezembro",
            _ => null
        };
        #pragma warning restore CS8603 // Possible null reference return.
    }



    public static bool IsValidCPForCNPJ(string? document)
    {
        if (string.IsNullOrWhiteSpace(document))
            return false;

        document = new string(document.Where(char.IsDigit).ToArray());

        return document.Length switch
        {
            11 => IsValidCPF(document),
            14 => IsValidCNPJ(document),
            _ => false
        };
    }

    public static bool IsValidCPF(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
            return false;

        if (cpf.Distinct().Count() == 1)
            return false;

        int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf[..9];
        int sum = 0;

        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

        int remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        string digit = remainder.ToString();
        tempCpf += digit;

        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        digit += remainder.ToString();

        return cpf.EndsWith(digit);
    }

    public static bool IsValidCNPJ(string? cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

        if (cnpj.Length != 14)
            return false;

        if (cnpj.Distinct().Count() == 1)
            return false;

        int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cnpj[..12];
        int sum = 0;

        for (int i = 0; i < 12; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];

        int remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        string digit = remainder.ToString();
        tempCnpj += digit;

        sum = 0;
        for (int i = 0; i < 13; i++)
            sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        digit += remainder.ToString();

        return cnpj.EndsWith(digit);
    }

    public static Boolean IsValidCellphone(string cellphone)
    {
        if (String.IsNullOrEmpty(cellphone))
        {
            return false;
        }

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

    public static string Slugify(string data, Boolean isAcceptingExtended = false, Boolean isRemoving = true, string keepChars = "")
    {
        if (String.IsNullOrEmpty(data))
            return data;

        string result = data;

        string from = "ãàáäâẽèéëêìíïîõòóöôùúüûñçÃÀÁÄÂẼÈÉËÊÌÍÏÎÕÒÓÖÔÙÚÜÛÑÇ";
        string to = "aaaaaeeeeeiiiiooooouuuuncAAAAAEEEEEIIIIOOOOOUUUUNC";
        string valid = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ ";
        string validExtended = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ -=_+|!@#$%&*()[]'\"/,.:;?\n\r";

        for (int i = 0, l = from.Length; i < l; i++)
        {
            result = result.Replace(from.Substring(i, 1), to.Substring(i, 1));
        }

        // remove invalid chars
        foreach (char c in result)
        {
            if (isAcceptingExtended ? !validExtended.Contains(c) : !valid.Contains(c))
            {
                if (!keepChars.Contains(c))
                {
                    result = isRemoving ? result.Replace(c.ToString(), String.Empty) : result.Replace(c, '-');
                }
            }
        }

        return result;
    }
}
