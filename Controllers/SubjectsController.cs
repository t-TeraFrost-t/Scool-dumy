using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School_D.Models;

namespace School_D.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly students_databaseContext _context;

        public SubjectsController(students_databaseContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Subject.Include(s => s.StudentSubject).ToListAsync());
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subject subject = await _context.Subject
                .Include(s => s.StudentSubject)
                .Include(s => s.IdGradeNavigation)
                .FirstOrDefaultAsync(m => m.IdSubject == id);
            var linq = _context.StudentSubject
                .Include(s => s.IdStudentNavigation)
                .Include(s => s.IdSubjectNavigation)
                .Where(s => s.IdSubject == subject.IdSubject)
                .ToHashSet();
            subject.StudentSubject = linq;
            var students = _context.Student.Where(s => !linq.Any(l => l.IdStudentNavigation.Name == s.Name)).Where(s => s.IdClassNavigation.IdGrade == subject.IdGradeNavigation.IdGrade).ToList();

             students.Select(s => s.Name = s.Name + " " + s.Surname);
            ViewBag.Students = new SelectList(students, "IdStudent", "Name");
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            ViewData["Grades"] = new SelectList(_context.Grade, "IdGrade", "Year");
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSubject,Name,IdGrade")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewBag.Grades = new SelectList(_context.Grade, "IdGrade", "Year",subject.IdGrade);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSubject,Name,IdGrade")] Subject subject)
        {
            if (id != subject.IdSubject)
            {
                return NotFound();
            }
            ICollection<StudentSubject> linq = _context.StudentSubject.Where(s => s.IdSubject == subject.IdSubject && s.IdStudentNavigation.IdClassNavigation.IdGrade != subject.IdGrade ).ToHashSet();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    _context.RemoveRange(linq);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.IdSubject))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subject
                .FirstOrDefaultAsync(m => m.IdSubject == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subject.FindAsync(id);
            _context.Subject.Remove(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subject.Any(e => e.IdSubject == id);
        }

        public async Task<IActionResult> AddStudent(int IdStudent, int IdSubject)
        {
            // var students =  _context.Students.Include(s => s.IdSchoolNavigation).Where(s => s.IdStudent == linq.IdStudent);
            StudentSubject linq = new StudentSubject();
            linq.IdStudent = IdStudent;
            linq.IdSubject = IdSubject;
            linq.IdStudentNavigation = _context.Student.FirstOrDefault(s => s.IdStudent == IdStudent);
            linq.IdSubjectNavigation = _context.Subject.FirstOrDefault(s => s.IdSubject == IdSubject);
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(linq);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = linq.IdSubject});
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                    //  ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                }


            }

            return RedirectToAction("Details", new { id = linq.IdSubject });
        }


        public async Task<IActionResult> DeleteStudent(int id)
        {

            var linq = _context.StudentSubject.FirstOrDefault(s => s.Id == id);
            //new SelectList(_context.School, "IdSchool", "Name", students.IdSchool);


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Remove(linq);
                    await _context.SaveChangesAsync();


                    return RedirectToAction("Details", new { id = linq.IdSubject });
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                    //  ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                }


            }



            return RedirectToAction("Details", new { id = linq.IdSubject });
        }
    }
}
