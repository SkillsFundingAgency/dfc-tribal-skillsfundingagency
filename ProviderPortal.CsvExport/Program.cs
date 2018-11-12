using Tribal.SkillsFundingAgency.ProviderPortal.CsvExport;

namespace ProviderPortal.CsvExport
{
    class Program
    {

        static void Main(string[] args)
        {
            new Processor().Process();
        }
    }
}
