namespace CourseProject.Core.DataAccess.Models
{
    public interface IEntity<T>
    {
        public T Id { get; set; }
    }
}