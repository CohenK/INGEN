using InGen.Services;
using System.Xml.Serialization;

namespace InGen.Services
{
    public static class CompanyService
    {
        static string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "InGen","CompanyInfo");
        static string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "InGen","CompanyInfo", "CompanyInfo.xml");
        static private Company company;
        static private XmlSerializer serializer;

        public static void Init()
        {
            serializer = new XmlSerializer(typeof(Company));
        }

        public static Company GetCompany()
        {
            Init();
            if (!Directory.Exists(directory))
            {
                DirectoryInfo resourceDirectory = new DirectoryInfo(directory);
                resourceDirectory.Create();
            }
            if (File.Exists(filePath))
            {
                TextReader reader = new StreamReader(filePath);
                object obj = serializer.Deserialize(reader);
                company = (Company)obj;
                reader.Close();
            }
            else
            {
                company = new Company();
                using (TextWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, company);
                }
            }
            return company;
        }

        public static void SetCompany()
        {
            Init();
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, company);
            }
        }
    }
}