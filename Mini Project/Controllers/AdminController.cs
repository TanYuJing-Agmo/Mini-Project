using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mini_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        /*
        // GET /api/admin/users
        [HttpGet]

        // Admin View All Enrollment List API
        // GET /api/admin/enrollments
        [HttpGet]
        
        // Admin Approve Student's Enrollment API
        // POST /api/admin/enrollments/approve
        [HttpPost]

        // Admin Reject Student's Enrollment API
        // POST /api/admin/enrollments/reject
        [HttpPost]*/
    }
}
