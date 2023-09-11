namespace Nop.Core.Domain.Omni_Backoffice
{
    public class StudentSummary : BaseEntity
    {
        public StudentSummary() { }

        public StudentSummary(string branch, string year, string term, int book, int course, int primary, int seconday) 
        {
            Branch = branch;
            Year = year;
            Term = term;
            Book = book;
            Course = course;
            Primary = primary;
            Secondary = seconday;
        }

        public string Branch { get; set; }

        public string Year { get; set; }

        public string Term { get; set; }

        public int Book { get; set; }

        public int Course { get; set; }

        public int Primary { get; set; }

        public int Secondary { get; set; }
    }
}
