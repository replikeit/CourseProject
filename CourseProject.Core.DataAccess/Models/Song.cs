namespace CourseProject.Core.DataAccess.Models
{
    public class Song : IEntity<int>
    {
        public int Id { get; set; }
        
        public string Tittle { get; set; }
        
        public string FilePath { get; set; }
        
        public Genre? Genre { get; set; }
    }
}