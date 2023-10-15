using System.Xml.Serialization;

namespace InGen.Services
{
    public class CompanyService
    {
        static string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "InGen");
        //static string filePath = directory + "\\companyInfo.xml";
        static string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "InGen", "CompanyInfo.xml");
        private Company company;
        private XmlSerializer serializer;

        public CompanyService()
        {
            serializer = new XmlSerializer(typeof(Company));
        }

        public Company GetCompany()
        {
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

        public void SetCompany()
        {
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, company);
            }
        }
    }
}