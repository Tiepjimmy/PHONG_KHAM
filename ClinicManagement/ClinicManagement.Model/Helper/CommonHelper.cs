namespace ClinicManagement.Model.Helper
{
    public class CommonHelper
    {
        public static string ConvertNumberToRoman(int value)
        {
            var output = string.Empty;
            switch (value)
            {
                case 1:
                    output = "I";
                    break;
                case 2:
                    output = "II";
                    break;
                case 3:
                    output = "III";
                    break;
                case 4:
                    output = "IV";
                    break;
                case 5:
                    output = "V";
                    break;
                case 6:
                    output = "VI";
                    break;
                case 7:
                    output = "VII";
                    break;
                case 8:
                    output = "VIII";
                    break;
                case 9:
                    output = "IX";
                    break;
                case 10:
                    output = "X";
                    break;
                case 11:
                    output = "XI";
                    break;
                case 12:
                    output = "XII";
                    break;
                case 13:
                    output = "XIII";
                    break;
                case 14:
                    output = "XIV";
                    break;
                case 15:
                    output = "XV";
                    break;
                case 16:
                    output = "XVI";
                    break;
                case 17:
                    output = "XVII";
                    break;
                case 18:
                    output = "XVIII";
                    break;
                case 19:
                    output = "XIX";
                    break;
                case 20:
                    output = "XX";
                    break;
                case 21:
                    output = "XXI";
                    break;
                case 22:
                    output = "XXII";
                    break;
                case 23:
                    output = "XXIII";
                    break;
                case 24:
                    output = "XXIV";
                    break;
                case 25:
                    output = "XXV";
                    break;
                case 26:
                    output = "XXVI";
                    break;
                case 27:
                    output = "XXVII";
                    break;
                case 28:
                    output = "XXVIII";
                    break;
                case 29:
                    output = "XXIX";
                    break;
                case 30:
                    output = "XXX";
                    break;
                default:
                    break;
            }
            return output;
        }

        public static string GetRandColor()
        {
            Random rnd = new();
            var hexOutput = string.Format("{0:X}", rnd.Next(0, 0xFFFFFF));
            while (hexOutput.Length < 6)
            {
                hexOutput = string.Concat(ConstantHelper.Zero, hexOutput);
            }
            return "#" + hexOutput;
        }

        public static string RemoveSign4VietnameseString(string str)
        {
            string[] vietnameseSigns =
            [

                "aAeEoOuUiIdDyY",

                "áàạảãâấầậẩẫăắằặẳẵ",

                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

                "éèẹẻẽêếềệểễ",

                "ÉÈẸẺẼÊẾỀỆỂỄ",

                "óòọỏõôốồộổỗơớờợởỡ",

                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

                "úùụủũưứừựửữ",

                "ÚÙỤỦŨƯỨỪỰỬỮ",

                "íìịỉĩ",

                "ÍÌỊỈĨ",

                "đ",

                "Đ",

                "ýỳỵỷỹ",

                "ÝỲỴỶỸ"
            ];
            for (int i = 1; i < vietnameseSigns.Length; i++)
            {
                for (int j = 0; j < vietnameseSigns[i].Length; j++)
                {
                    str = str.Replace(vietnameseSigns[i][j], vietnameseSigns[0][i - 1]);
                }
            }
            return str;
        }

        public static int FindAndExtractNumber(string str)
        {
            string b = string.Empty;
            int val = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str[i]))
                {
                    b += str[i];
                }
            }

            if (b.Length > 0)
            {
                val = int.Parse(b);
            }
            return val;
        }
    }
}
