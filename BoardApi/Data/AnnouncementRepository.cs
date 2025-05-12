using System.Data;
using Microsoft.Data.SqlClient;
using BoardApi.Models;

namespace BoardApi.Data
{
    public class AnnouncementRepository
    {
        private readonly IConfiguration _configuration;

        public AnnouncementRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<Announcement>> GetAllAsync()
        {
            var list = new List<Announcement>();
            using var conn = GetConnection();
            using var cmd = new SqlCommand("GetAllAnnouncements", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new Announcement
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    CreatedDate = reader.GetDateTime(3),
                    Status = reader.GetBoolean(4),
                    Category = reader.GetString(5),
                    SubCategory = reader.GetString(6)
                });
            }
            return list;
        }

        public async Task<Announcement?> GetByIdAsync(int id)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("GetAnnouncementById", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Announcement
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    CreatedDate = reader.GetDateTime(3),
                    Status = reader.GetBoolean(4),
                    Category = reader.GetString(5),
                    SubCategory = reader.GetString(6)
                };
            }
            return null;
        }

        public async Task AddAsync(Announcement announcement)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("InsertAnnouncement", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Title", announcement.Title);
            cmd.Parameters.AddWithValue("@Description", announcement.Description);
            cmd.Parameters.AddWithValue("@Status", announcement.Status);
            cmd.Parameters.AddWithValue("@Category", announcement.Category);
            cmd.Parameters.AddWithValue("@SubCategory", announcement.SubCategory);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Announcement announcement)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("UpdateAnnouncement", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", announcement.Id);
            cmd.Parameters.AddWithValue("@Title", announcement.Title);
            cmd.Parameters.AddWithValue("@Description", announcement.Description);
            cmd.Parameters.AddWithValue("@Status", announcement.Status);
            cmd.Parameters.AddWithValue("@Category", announcement.Category);
            cmd.Parameters.AddWithValue("@SubCategory", announcement.SubCategory);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var conn = GetConnection();
            using var cmd = new SqlCommand("DeleteAnnouncement", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
