using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObjects.Models;
using Services.Interface;
using System.Security.Claims;

namespace ClubManagementSystem.Controllers
{
    public class TaskAssignmentsController : Controller
    {
        private readonly FptclubsContext _context;
        private readonly ITaskAssignmentService _taskAssignmentService;

        public TaskAssignmentsController(ITaskAssignmentService taskAssignmentService)
        {
            _taskAssignmentService = taskAssignmentService;
        }

        public async Task<IActionResult> Assign(int taskID, int memberID)
        {
            var isAssigned = await _taskAssignmentService.IsAssignedBefore(memberID, taskID);
            if (isAssigned)
                return RedirectToAction("Details", "ClubTasks", new { id = taskID, error = "Task is already assigned for this member" });
            TaskAssignment assign = new TaskAssignment()
            {
                TaskId = taskID,
                MembershipId = memberID
            };
            await _taskAssignmentService.AddTaskAssignmentAsync(assign);
            return RedirectToAction("Details", "ClubTasks", new { id = taskID });
        }

        public async Task<IActionResult> Receive(int assignID, string status)
        {
            var assign = await _taskAssignmentService.GetTaskAssignmentAsync(assignID);
            assign.Status = status;
            await _taskAssignmentService.UpdateTaskAssignmentAsync(assign);
            return RedirectToAction("Index", "TaskAssignments");
        }

        // GET: TaskAssignments
        public async Task<IActionResult> Index()
        {
            var userID = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var tasks = await _taskAssignmentService.GetPersonalTaskAssignmentsAsync(userID);
            return View(tasks);
        }
    }
}
