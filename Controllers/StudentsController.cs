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
    public class StudentsController : Controller
    {
        private  students_databaseContext _context;

        public StudentsController(students_databaseContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(int? ids=null)
        {
            //

             var students_databaseContext = ids is null ? _context.Student.Include(s => s.IdClassNavigation).Include(s => s.StudentSubject) : _context.Student.Include(s => s.IdClassNavigation).Include(s => s.StudentSubject).Where(s => s.IdClassNavigation.IdGradeNavigation.IdSchool == ids);
            foreach (Student s in students_databaseContext)
            {
                s.IdClassNavigation = _context.Class.Include(c => c.IdGradeNavigation).FirstOrDefault(c => c.IdClass == s.IdClass);
            }
           
            //var students_databaseContext = _context.Student.Include(s => s.IdClassNavigation).Include(s => s.StudentSubject);
            var str = await students_databaseContext.ToListAsync();
            
            return View(str);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.Student
                .Include(s => s.IdClassNavigation)
                .Include(s => s.StudentSubject)
                .FirstOrDefaultAsync(m => m.IdStudent == id);
            var linq = _context.StudentSubject
                .Include(s => s.IdStudentNavigation)
                .Include(s => s.IdSubjectNavigation)
                .Where(s => s.IdStudent == students.IdStudent)
                .ToHashSet();
            students.StudentSubject = linq;
            var subjects = _context.Subject.Where(s => !linq.Any(l => l.IdSubjectNavigation.Name == s.Name)).Where(s => s.IdGradeNavigation.IdGrade == students.IdClassNavigation.IdGrade).ToList();
            ViewBag.Subjects = new SelectList( subjects, "IdSubject", "Name"); ;
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // GET: Students/Create
        public IActionResult Create()
        {

            
            ViewData["Classes"] = new SelectList(_context.Class, "IdClass", "Letter");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStudent,Name,Surname,Age,IdClass")] Student students)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(students);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                  //  ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                }


            }
            
            ViewData["IdClass"] = new SelectList(_context.Class, "IdClass", "Letter", students.IdClass);
            return View(students);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.Student.FindAsync(id);
            if (students == null)
            {
                return NotFound();
            }
            var con = _context.Class.Include(c => c.IdGradeNavigation).ToList();
            con.Select(c => c.Letter = c.Letter + "-" + c.IdGradeNavigation.Year.ToString()).ToList();
            ViewData["Classes"] = new SelectList(con, "IdClass", "Letter", students.IdClass);
            return View(students);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStudent,Name,Surname,Age,IdClass")] Student students)
        {
            if (id != students.IdStudent)
            {
                return NotFound();
            }
            students.IdClassNavigation = await _context.Class.FirstOrDefaultAsync(c => c.IdClass == students.IdClass);
            ICollection<StudentSubject> linq = _context.StudentSubject.Include(s => s.IdSubjectNavigation).Where(s => s.IdStudent == students.IdStudent && s.IdSubjectNavigation.IdGrade != students.IdClassNavigation.IdGrade).ToHashSet();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(students);
                    _context.RemoveRange(linq);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentsExists(students.IdStudent))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception e)
                {
                    throw;
                 //   return ViewBag.Message = e.Message.ToString();
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["Classes"] = new SelectList(_context.Class, "IdClass", "Letter", students.IdClass);
            return View(students);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var students = await _context.Student
                .Include(s => s.IdClassNavigation)
                .FirstOrDefaultAsync(m => m.IdStudent == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }
       
        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var students = await _context.Student.FindAsync(id);
            _context.Student.Remove(students);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentsExists(int id)
        {
            return _context.Student.Any(e => e.IdStudent == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubjects(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var students = await _context.Student.Include(s => s.StudentSubject).FirstOrDefaultAsync(m => m.IdStudent == id);

            if (students == null)
            {
                return NotFound();
            }
            return View("/Students/EditSubjects", students);
        }

        
        public async Task<IActionResult> AddSubject(int IdStudent,int IdSubject)
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
                    return RedirectToAction("Details", new { id = linq.IdStudent });
                }
                catch (Exception e)
                {
                    //ViewBag.Message = e.Message;
                    //  ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                }


            }

            return RedirectToAction("Details", new { id = linq.IdStudent });
        }

       
        public async Task<IActionResult> DeleteSubject(int id)
        {
           
            var linq = _context.StudentSubject.FirstOrDefault(s => s.Id == id);
            //new SelectList(_context.School, "IdSchool", "Name", students.IdSchool);

         
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Remove(linq);
                    await _context.SaveChangesAsync();
                    
                   
                    return RedirectToAction("Details", new { id = linq.IdStudent });
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                    //  ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                }


            }


            
            return RedirectToAction("Details",new { id = linq.IdStudent });
        }

        
    }
}
