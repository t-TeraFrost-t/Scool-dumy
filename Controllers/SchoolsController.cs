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
    public class SchoolsController : Controller
    {
        private readonly students_databaseContext _context;

        public SchoolsController(students_databaseContext context)
        {
            _context = context;
        }

        // GET: Schools
        public async Task<IActionResult> Index()
        {
            var school_databaseContext = _context.School;
            foreach (School s in school_databaseContext)
            {
                s.Grade = _context.Grade.Include(g => g.Class).Where(c => c.IdSchool == s.IdSchool).ToHashSet();
                foreach (Grade g in s.Grade)
                {
                    g.Class = _context.Class.Include(c => c.Student).Where(c => c.IdGrade == g.IdGrade).ToHashSet();

                }
            }
            return View(await school_databaseContext.ToListAsync());
            
        }

        // GET: Schools/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var school = await _context.School
                .FirstOrDefaultAsync(m => m.IdSchool == id);
            if (school == null)
            {
                return NotFound();
            }

            return View(school);
        }

        // GET: Schools/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Schools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSchool,Name")] School school)
        {
            
         
        if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Add(school);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ViewBag.Message = e.Message;
                }
                
            }
            return View(school);
        }

        // GET: Schools/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var school = await _context.School.FindAsync(id);
            if (school == null)
            {
                return NotFound();
            }
            return View(school);
        }

        // POST: Schools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSchool,Name")] School school)
        {
            if (id != school.IdSchool)
            {
                return NotFound();
            }

            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(school);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolExists(school.IdSchool))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }catch(Exception e)
                {
                    return ViewBag.Message = e.Message;
                }
                
            }
            return View(school);
        }

        // GET: Schools/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var school = await _context.School
                .FirstOrDefaultAsync(m => m.IdSchool == id);
            if (school == null)
            {
                return NotFound();
            }

            return View(school);
        }

        // POST: Schools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var school = await _context.School.FindAsync(id);
            _context.School.Remove(school);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchoolExists(int id)
        {
            return _context.School.Any(e => e.IdSchool == id);
        }
        public async Task<IActionResult> ShowStudents(int? id)
        {
            // Edit(int id, [Bind("IdSchool,Name")] School school)
            if (id == null)
            {
                return NotFound();
            }

            var school = await _context.School.Include(m => m.Grade).FirstOrDefaultAsync(m => m.IdSchool == id); ;
                
            if (school == null)
            {
                return NotFound();
            }

            //return RedirectToPage("/Students/Index", school.Students);
            
            return Redirect($"/Students?ids={id}");
        }
    }
}
