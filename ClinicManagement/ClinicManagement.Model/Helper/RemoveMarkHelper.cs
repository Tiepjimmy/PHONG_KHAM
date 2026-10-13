using System.Text;
using System.Text.RegularExpressions;

namespace ClinicManagement.Model.Helper
{
    public partial class RemoveMarkHelper
    {
        public static string ConvertToUnSign(string s)
        {
            Regex regex = MyRegex();
            var temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        [GeneratedRegex("\\p{IsCombiningDiacriticalMarks}+")]
        private static partial Regex MyRegex();
    }
}
