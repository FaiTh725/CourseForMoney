using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Services.Interfaces;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace StudentPlacement.Backend.Services.Implementations
{
    public class DocService : IDocService
    {
        private readonly IStudentRepository studentRepository;

        public DocService(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        public async Task<DocX> CreateReport(int idGroup)
        {
            var students = await studentRepository.GetStudentForReport(idGroup);

            DocX document = DocX.Create("Allocation.docx");

            document.SetDefaultFont(new Font("Times New Roman"), 14);

            Table table = document.AddTable(students.Count + 1, 5);
            table.Alignment = Alignment.center;
            table.Design = TableDesign.TableGrid;

            table.Rows[0].Cells[0].Paragraphs[0].Append("ФИО").Bold().Alignment = Alignment.center;
            table.Rows[0].Cells[1].Paragraphs[0].Append("Средний балл").Bold().Alignment = Alignment.center;
            table.Rows[0].Cells[2].Paragraphs[0].Append("Организация").Bold().Alignment = Alignment.center;
            table.Rows[0].Cells[3].Paragraphs[0].Append("Контакты").Bold().Alignment = Alignment.center;
            table.Rows[0].Cells[4].Paragraphs[0].Append("Адрес заявки").Bold().Alignment = Alignment.center;

            for (int i = 0; i < students.Count; i++)
            {
                table.Rows[i + 1].Cells[0].Paragraphs[0].Append(students[i].FullName).Alignment = Alignment.center;
                table.Rows[i + 1].Cells[1].Paragraphs[0].Append(students[i].AverageScore.ToString()).Alignment = Alignment.center;
                if (students[i].AllocationData.NameOrganixation != null)
                {
                    table.Rows[i + 1].Cells[2].Paragraphs[0].Append(students[i].AllocationData.NameOrganixation).Alignment = Alignment.center;
                    table.Rows[i + 1].Cells[3].Paragraphs[0].Append(students[i].AllocationData.Contacts).Alignment = Alignment.center;
                    table.Rows[i + 1].Cells[4].Paragraphs[0].Append(students[i].AllocationData.AdressRequest).Alignment = Alignment.center;
                }
                else
                {
                    table.Rows[i + 1].MergeCells(2,4);
                    table.Rows[i + 1].Cells[2].Paragraphs[0].Append("Не распределен").Alignment = Alignment.center;
                }

            }

            document.InsertParagraph().InsertTableAfterSelf(table);

            return document;
        }
    }
}
