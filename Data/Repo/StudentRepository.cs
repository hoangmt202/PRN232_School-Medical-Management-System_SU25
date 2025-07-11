using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccess.Repo
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(SchoolMedicalDbContext context) : base(context) { }
        public async Task<List<Student>> GetEligibleStudentsForPlanAsync(VaccinationPlan plan)
        {
            var eligibleGrades = ParseTargetGroup(plan.TargetGroup);

            var allStudents = await _dbSet.ToListAsync();
            return allStudents
                .Where(s => eligibleGrades.Contains(ParseGradeNumber(s.Class)))
                .ToList();
        }

        private List<int> ParseTargetGroup(string targetGroup)
        {
            // Examples: "Grade 1 (6 y/o)", "All Grades 1-5"
            var grades = new List<int>();

            var match = Regex.Match(targetGroup, @"Grade\s+(\d)");
            if (match.Success)
            {
                grades.Add(int.Parse(match.Groups[1].Value));
                return grades;
            }

            var allMatch = Regex.Match(targetGroup, @"Grades\s*(\d)\s*-\s*(\d)");
            if (allMatch.Success)
            {
                int start = int.Parse(allMatch.Groups[1].Value);
                int end = int.Parse(allMatch.Groups[2].Value);
                for (int i = start; i <= end; i++) grades.Add(i);
            }

            return grades;
        }

        private int ParseGradeNumber(string studentClass)
        {
            var match = Regex.Match(studentClass, @"Grade\s*(\d)");
            return match.Success ? int.Parse(match.Groups[1].Value) : -1;
        }
        public async Task<IEnumerable<Student>> GetByParentIdAsync(int parentId)
        {
            return await _dbSet
                .Where(s => s.ParentId == parentId)
                .ToListAsync();
        }
    }
}
