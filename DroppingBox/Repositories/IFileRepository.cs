using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;

namespace DroppingBox.Models
{
    public interface IFileRepository
    {

        List<File> GetAll();
        //User GetByEmail(string email);
        void Upload(File file);

    }

    public class MemoryFileRepository : IFileRepository
    {
        private List<File> files
                    = new List<File>
                    {
                        new File
                        {
                            FileName = "image.jpg",
                            FileLink = null,
                            Comment = "Good"                            
                        },
                    };

        public List<File> GetAll() { return files; }

        //public User GetByEmail(string email) {
        //    return users.Find(u => u.Email.Equals(email)); }

        public void Upload(File file) {
            files.Add(file);
        }

    }
}
