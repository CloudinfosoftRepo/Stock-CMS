using Microsoft.Data.SqlClient;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Service
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly string _connectionString;

        public FileService(ILogger<FileService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DBConnection");
        }

        public void CreateBackup(string backupPath)
        {
            if (File.Exists(backupPath))
            {
                File.Delete(backupPath);
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $@"BACKUP DATABASE dm_cms
                                TO DISK = N'{backupPath}'
                                WITH NOFORMAT, NOINIT,
                                NAME = N'dm_cms-Full Database Backup',
                                SKIP, NOREWIND, NOUNLOAD, STATS = 10;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

        }
    }
}
