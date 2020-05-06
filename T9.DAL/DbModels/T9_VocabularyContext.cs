namespace T9.DAL.DbModels
{
    using System;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using T9.DAL.DbInitializers;

    public class T9_VocabularyContext : DbContext
    {
        public T9_VocabularyContext()
            : base("name=T9_VocabularyContext")
        {
            Database.SetInitializer(new MyDbInitializer());
        }

        public DbSet<Word> Words { get; set; }
    }
}