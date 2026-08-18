

using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace NewGaras.Domain.Helper
{
    public static class Extension
    {
        static IExcelDataReader _reader;

        public static string SearchedKey(string Keyword)
        {

            string SearchedKey = "%";

            foreach(var c in Keyword)
            {
                switch(c)
                {
                    case 'أ':
                    case 'ا':
                    case 'إ':
                    case 'ء':
                        SearchedKey+="[ءاأإآ]";
                        break;

                    case 'ه':
                    case 'ة':
                        SearchedKey+="[ه|ة]";
                        break;

                    case 'ي':
                    case 'ى':
                    case 'ئ':
                        SearchedKey+="[ى|ي|ئ]";
                        break;

                    case ' ':
                        SearchedKey+="%%";
                        break;

                    case 'و':
                    case 'ؤ':
                        SearchedKey+="[و|ؤ]";
                        break;

                    default:
                        SearchedKey+=c;
                        break;
                }
            }
            SearchedKey+="%";

            return SearchedKey;

        }
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if(dob.Date>today.AddYears(-age))
                age--;
            return age;
        }

        public static DataSet CreateDsOfExcelFile(IFormFile file , string dirPath)
        {
            // Create the directory if it is not exist
            if(!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // Make sure that only Excel file is used 
            string dataFileName = Path.GetFileName(file.FileName);

            string extension = Path.GetExtension(dataFileName);

            string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

            if(!allowedExtsnions.Contains(extension))
                throw new Exception("Sorry! This file is not allowed, make sure that file having extension as either.xls or.xlsx is uploaded.");

            // Make a Copy of the Posted File from the Received HTTP Request
            string saveToPath = Path.Combine(dirPath, dataFileName);

            using(FileStream stream = new FileStream(saveToPath , FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Use this to handle Encodeing differences in .NET Core
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            DataSet ds = new DataSet();
            // Read the excel file
            using(var stream = new FileStream(saveToPath , FileMode.Open))
            {
                if(extension==".xls")
                    _reader=ExcelReaderFactory.CreateBinaryReader(stream);
                else
                    _reader=ExcelReaderFactory.CreateOpenXmlReader(stream);

                ds=_reader.AsDataSet();
                _reader.Close();
            }

            File.Delete(saveToPath);

            return ds;
        }

    }
}
