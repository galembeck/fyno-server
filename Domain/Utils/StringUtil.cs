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



    public static bool IsValidCNPJ(string CNPJ) 
    {
        try
        {
            CNPJ = StringUtil.Slugify(CNPJ, isRemoving: true);
            CNPJ = CNPJ.Replace(" ", "");
            Int64 number;
            bool isNumber = Int64.TryParse(CNPJ, out number);

            if (String.IsNullOrEmpty(CNPJ) || !isNumber) 
            {
                return false;
            }

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma;
            int resto;
            string digito;
            string tempCnpj;
            CNPJ = CNPJ.Trim();
            CNPJ = CNPJ.Replace(".", "").Replace("-", "").Replace("/", "");
            if (CNPJ.Length != 14)
                return false;
            tempCnpj = CNPJ.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return CNPJ.EndsWith(digito);
        }
        catch
        {
            return false;
        }
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
