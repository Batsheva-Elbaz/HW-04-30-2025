using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_04_30_2025.Data
{
    public class Repository
    {
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Image> GetAll()
        {
            using var ctx = new ImageUploadContext(_connectionString);
            return ctx.Images.OrderByDescending(i => i.UploadedDate).ToList();
        }

        public void Add(string title, string fileName)
        {
            using var ctx = new ImageUploadContext(_connectionString);
            ctx.Images.Add(new Image
            {
                Title = title,
                FileName = fileName,
                UploadedDate = DateTime.Now,
                Likes = 0
            });
            ctx.SaveChanges();

        }

        public Image GetById(int id)
        {
            using var ctx = new ImageUploadContext(_connectionString);
            return ctx.Images.FirstOrDefault(i => i.Id == id);
        }

        public void IncrementLikes(int id)
        {
            using var ctx = new ImageUploadContext(_connectionString);
            var image = ctx.Images.FirstOrDefault(i => i.Id == id);
            if (image != null)
            {
                image.Likes++;
                ctx.SaveChanges();
            }
        }

        public int GetLikes(int id)
        {
            using var ctx = new ImageUploadContext(_connectionString);
            return ctx.Images.First(i => i.Id == id).Likes;

        }
    }
}
