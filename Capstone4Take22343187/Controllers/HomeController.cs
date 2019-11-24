using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Capstone4Take22343187.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Capstone4Take22343187.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TaskListDBContext _context;
        public HomeController(ILogger<HomeController> logger, TaskListDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize]
        public IActionResult TasksLists()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (_context.AspNetUsers.Where(user => user.Id == id) != null)
            {
                return View(_context.Tasks.Where(tasks => tasks.OwnerId == id).ToList());
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet]
        public IActionResult AddTask()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTask(Tasks taskToAdd)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(taskToAdd);
                _context.SaveChanges();
                return RedirectToAction("TasksLists");
            }
            return View();
        }
        public IActionResult DeleteTask(int id)
        {
            var foundTasks = _context.Tasks.Find(id);
            if (foundTasks != null)
            {
                _context.Tasks.Remove(foundTasks);
                _context.SaveChanges();
            }
            return RedirectToAction("TasksLists");
        }
        [HttpPost]
        public IActionResult UpdateTask(Tasks newTasks)
        {
            if (ModelState.IsValid)
            {
                Tasks oldTasks = _context.Tasks.Find(newTasks.TaskId);
                oldTasks.TaskName = newTasks.TaskName;
                oldTasks.TaskDescription = newTasks.TaskDescription;
                oldTasks.DueDate = newTasks.DueDate;

                _context.Entry(oldTasks).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _context.Update(oldTasks);
                _context.SaveChanges();
            }
            return RedirectToAction("TasksLists");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
