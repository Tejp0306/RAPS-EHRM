using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Helpers;
using EHRM.ServiceLayer.LeaveDashBoard;
using EHRM.ServiceLayer.LeaveTypes;
using EHRM.ServiceLayer.Master;
using EHRM.ViewModel.EmployeeDeclaration;
using EHRM.ViewModel.Leave;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.Review;
using EHRM.ViewModel.Self;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace EHRM.Web.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ILeaveTypes _leave;
        private readonly ILeaveDashboardService _leaveDashboardService;
        private object _emailService;
        private readonly IUnitOfWork _UnitOfWork;



        public LeaveController(ILeaveTypes leave, ILeaveDashboardService leaveDashboard, IUnitOfWork unitOfWork)
        {

            _leave = leave;
            _leaveDashboardService = leaveDashboard;
            _UnitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddLeaveType()
        {
            return View();
        }
        public IActionResult LeaveApply()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var name = userDetails.userName;
            ViewData["Name"] = name;
            return View();
        }

        //public IActionResult LeaveStatus()
        //{
        //    return View();
        //}

        #region AddLeaveType

        [HttpPost]
        public async Task<IActionResult> SaveLeaveType(LeaveTypeViewModel model)
        {  // Check if the exists based on the model ID
            if (model.Id > 0)
            {
                // Update the role details
                //string updatedBy = "waseem"; // Replace with actual logic to fetch the current user ID
                var updateResult = await UpdateLeaveTypeDetails(model.Id, model);

                if (updateResult != null)
                {

                    var updateResponse = updateResult as dynamic; // Assuming it's returning an anonymous type
                    if (updateResponse?.success == true)
                    {
                        TempData["ToastType"] = "success"; // Store success message
                        TempData["ToastMessage"] = "Record has been updated ";
                        return RedirectToAction("AddLeaveType"); // Redirect to the list of roles
                    }
                    else
                    {
                        TempData["ToastType"] = "danger"; // Store error message
                        TempData["ToastMessage"] = "An error occurred while updating the record.";
                        return RedirectToAction("AddLeaveType");  // Return to the same view with the provided model
                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Unexpected error occurred during Employee Type update."; // Display generic error
                    return View(model); // Return to the same view with the provided model
                }
            }
            else
            {
                // Create a new role
                // string createdById = "waseem"; // Replace with logic to fetch the actual user ID
                var result = await _leave.CreateLeaveTypeAsync(model);

                // Handle the result of the create operation
                if (result.Success)
                {
                    // Error handling for the case where creation fails
                    TempData["ToastType"] = "success";  // Success, danger, warning, info
                    TempData["ToastMessage"] = "Record saved successfully!";
                    return RedirectToAction("AddLeaveType"); // Redirect to the list of roles
                }
                else
                {
                    // Error handling for the case where creation fails
                    TempData["ToastType"] = "danger"; // Store error message
                    TempData["ToastMessage"] = "An error occurred while creating the record.";
                    return RedirectToAction("AddLeaveType"); // Redirect back to the EmployeeType view
                }
            }
        }

        [NonAction]
        private async Task<object> UpdateLeaveTypeDetails(int id, LeaveTypeViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _leave.UpdateLeaveTypeAsync(id, model);

                // Return a structured response based on the result of the update
                return new
                {
                    success = result.Success,
                    message = result.Message
                };
            }
            catch (Exception)
            {
                return new
                {
                    success = false,
                    message = "An error occurred while updating the Employee Type. Please try again later."
                };
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAllLeaveTypeData()
        {
            // Fetch the result from the service layer
            var result = await _leave.GetAllLeaveTypeAsync();

            // Check if the result is successful and contains data
            if (result.Success && result.Data != null)
            {
                var leavetype = result.Data as IEnumerable<Leavetypee>;
                if (leavetype != null)
                {
                    // Return the list of roles as a JSON response
                    var leaveTypeList = leavetype.Select(leavetype => new
                    {
                        Id = leavetype.Id,
                        LeaveType = leavetype.LeaveType,
                        LeaveDescription = leavetype.LeaveDescription,
                    }).ToList();
                    return Json(leaveTypeList);
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in expected format." });
                }
            }
            else
            {
                // Handle the case where the service failed
                return Json(new { Success = false, Message = result.Message ?? "No Employee Type found." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLeaveType(int id)
        {
            try
            {
                // Call the service method to delete the notice from the database
                var result = await _leave.DeleteLeaveTypeAsync(id);

                // Check the result and provide feedback to the user
                if (result.Success)
                {
                    return Json(new { Success = true, Message = "Leave_Type deleted successfully!" });
                }
                else
                {
                    return Json(new { Success = true, Message = "Leave_Type not deleted !" });
                }


            }
            catch (Exception ex)
            {
                // Store the error message in TempData if an exception occurs
                TempData["ErrorMessage"] = $"Error deleting the Leave_Type: {ex.Message}";
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        [HttpGet("Leave/GetLeaveTypeDetails/{leaveTypeID}")]
        public async Task<JsonResult> GetLeaveTypeDetails([FromRoute] int leaveTypeID)
        {
            try
            {
                var asset = await _leave.GetLeaveTypeByIdAsync(leaveTypeID);

                if (asset == null)
                {
                    return Json(new { success = false, message = "Asset not found." });
                }

                return Json(new { success = true, data = asset });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while retrieving the Asset details." });
            }
        }

        #endregion


        #region Leave Apply

        [HttpGet]
        public async Task<JsonResult> GetLeaveTypeData()
        {
            // Fetch the result from the service layer
            var result = await _leave.GetLeaveTypeAsync();

            if (result.Success && result.Data != null)
            {
                // Attempt to cast result.Data to IEnumerable<Team>
                if (result.Data is IEnumerable<Leavetypee> leaveType)
                {
                    // Project the team list to a simplified JSON-friendly format
                    var leaveTypeList = leaveType.Select(leave => new
                    {
                        id = leave.Id, // Ensure Team class has an Id property
                        name = leave.LeaveType // Ensure Team class has a Name property
                    }).ToList();

                    return Json(new { Success = true, Data = leaveTypeList });
                }
                else
                {
                    return Json(new { Success = false, Message = "Data is not in the expected format (IEnumerable<Team>)." });
                }
            }
            else
            {
                // Handle failure scenarios
                return Json(new { Success = false, Message = result.Message ?? "No teams found." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveLeaveApply(LeaveApplyViewModel model)
        {
            // Retrieve the JWT token from the session
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");

            // Extract user details from the JWT token
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var empId = userDetails.userId;

            // Set EmpId for the model
            model.EmpId = Convert.ToInt32(empId);

            // Check if the model has a valid ID (to decide whether to update or create a new leave record)
            if (model.Id > 0)
            {
                // Update the leave details
                var updateResult = await UpdateLeaveApplyDetails(model.Id, model);

                if (updateResult != null)
                {
                    var updateResponse = updateResult as dynamic;
                    if (updateResponse?.success == true)
                    {
                        TempData["ToastType"] = "success"; // Success message
                        TempData["ToastMessage"] = "Record has been updated ";
                        return RedirectToAction("LeaveApply");
                    }
                    else
                    {
                        TempData["ToastType"] = "danger"; // Error message
                        TempData["ToastMessage"] = "An error occurred while updating the record.";
                        return RedirectToAction("LeaveApply");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Unexpected error occurred during Employee Type update.";
                    return View(model);
                }
            }
            else
            {
                // Create a new leave application
                var result = await _leave.LeaveApplyAsync(model);

                // Handle the result of the create operation
                if (result.Success)
                {
                    TempData["ToastType"] = "success";  // Success message
                    TempData["ToastMessage"] = "Record saved successfully!";
                    return RedirectToAction("LeaveApply");
                }
                else
                {
                    // Error handling if the creation fails
                    TempData["ToastType"] = "danger"; // Error message
                    TempData["ToastMessage"] = "An error occurred while creating the record.";
                    return RedirectToAction("LeaveApply");
                }
            }
        }

        [NonAction]
        private async Task<object> UpdateLeaveApplyDetails(int id, LeaveApplyViewModel model)
        {
            try
            {
                // Call the service method to update the role
                var result = await _leave.UpdateLeaveApplyAsync(id, model);

                // Return a structured response based on the result of the update
                return new
                {
                    success = result.Success,
                    message = result.Message
                };
            }
            catch (Exception)
            {
                return new
                {
                    success = false,
                    message = "An error occurred while updating the Employee Type. Please try again later."
                };
            }
        }

        public async Task<IActionResult> GetLeaveDetails()
        {
            // Retrieve the JWT token from the session
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");

            // Extract user details from the JWT token
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);

            // Set EmpId based on extracted user details
            int empId = Convert.ToInt32(userDetails.userId);

            // Fetch leave data for the employee
            var leaveData = await _leave.GetLeaveDataByEmpId(empId);

            // Return leave data as JSON array
            return Json(leaveData);
        }

        #endregion


        #region Leave Status

        //[HttpGet]
        //public async Task<JsonResult> GetLeaveData()
        //{
        //    // Fetch the result from the service layer
        //    var result = await _leave.GetLeaveAsync();

        //    // Check if the result is successful and contains data
        //    if (result.Success && result.Data != null)
        //    {
        //        var leavetype = result.Data as IEnumerable<LeaveApply>;
        //        if (leavetype != null)
        //        {
        //            // Return the list of roles as a JSON response
        //            var leaveTypeList = leavetype.Select(leavetype => new
        //            {
        //                Id = leavetype.Id,
        //                EmployeeName = leavetype.EmployeeName,
        //                LeaveType = leavetype.LeaveType,
        //                LeaveFrom = leavetype.LeaveFrom,
        //                LeaveTo = leavetype.LeaveTo,
        //            }).ToList();
        //            return Json(leaveTypeList);
        //        }
        //        else
        //        {
        //            return Json(new { Success = false, Message = "Data is not in expected format." });
        //        }
        //    }
        //    else
        //    {
        //        // Handle the case where the service failed
        //        return Json(new { Success = false, Message = result.Message ?? "No Employee Type found." });
        //    }
        //}

        //Working
        public async Task<IActionResult> LeaveStatus()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var roleid = userDetails.roleId;
            var empid = userDetails.userId;
            var applyRepository = _UnitOfWork.GetRepository<LeaveApply>();
            var statusRepository = _UnitOfWork.GetRepository<LeaveStatuss>();
            var employmentRepository = _UnitOfWork.GetRepository<EmployementTypeDetail>();

            var apply = await applyRepository.GetAllAsync();
            var status = await statusRepository.GetAllAsync();
            var employment = await employmentRepository.GetAllAsync();


            List<LeaveStatusViewModel> leaveRequests = new List<LeaveStatusViewModel>();


            if (roleid == 1) // HR/Admin sees all leave requests
            {
                leaveRequests = (from leave in apply
                                 join stat in status
                                 on leave.Id equals stat.LeaveId
                                 select new LeaveStatusViewModel
                {
                                     Id = (int)leave.Id,
                                     EmployeeName = leave.EmployeeName,
                                     LeaveType = leave.LeaveType,
                                     LeaveFrom = leave.LeaveFrom,
                                     TillDate = leave.LeaveTo,
                                     LeaveStatus = stat.LeaveStatus,
                                     ManagerRemark = stat.ManagerRemark,
                                     TotalDays = (int)leave.TotalDays,

                    }).ToList();
                    //return Json(leaveRequests);
                }
            else if (roleid == 4) // Managers see only pending requests of their employees
                {
                leaveRequests = (from leave in apply
                                 join emp in employment
                                 on leave.EmpId equals emp.EmpId
                                 join stat in status
                                 on leave.Id equals stat.LeaveId
                                 where emp.ManagerId == Convert.ToInt32(empid) && stat.LeaveStatus == "Pending"
                                 select new LeaveStatusViewModel
            {
                                     Id = (int)leave.Id,
                                     EmployeeName = leave.EmployeeName,
                                     LeaveType = leave.LeaveType,
                                     LeaveFrom = leave.LeaveFrom,
                                     TillDate = leave.LeaveTo,
                                     LeaveStatus = stat.LeaveStatus,
                                     ManagerRemark = stat.ManagerRemark,
                                     TotalDays = (int)leave.TotalDays,

                                 }).ToList();
            }

            LeaveStatusViewModel leaveStatus = new LeaveStatusViewModel();
            leaveStatus.Items = leaveRequests;


            return View(leaveStatus);
        }


        [HttpPost]
        public async Task<IActionResult> SubmitLeaveStatus(LeaveStatusViewModel model)
        {
            // Retrieve the JWT token from the session
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");

            // Extract user details from the JWT token
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            var empId = userDetails.userId;

            // Set EmpId for the model
            model.EmpId = Convert.ToInt32(empId);

            // Call the service method to create a new leave status
            var result = await _leave.UpdateLeaveStatusAsync(model.Id, model);


            // Handle the result of the create operation
            if (result.Data == "Approved")
            {
                var leaveApplyRepository = _UnitOfWork.GetRepository<LeaveApply>();

                // Fetch the LeaveApply record where Id matches the given leaveId
                var leaveRecord = await leaveApplyRepository.GetByIdAsync(model.Id);
                if (leaveRecord == null)
                {
                    throw new Exception("Leave record not found.");
                }

                int EmpId = leaveRecord.EmpId ?? 0;
                int leaveCount = leaveRecord.TotalDays ?? 0;
                string leaveType = leaveRecord.LeaveType;

                var leaveBalanceRepository = _UnitOfWork.GetRepository<LeaveBalance>();

                // Fetch the Employee Leave Balance
                var leaveBalances = await leaveBalanceRepository.GetAllAsync();
                var leaveBalance = leaveBalances.FirstOrDefault(lb => lb.EmpId == EmpId);
                if (leaveBalance == null)
                {
                    throw new Exception("Leave balance record not found for the employee.");
                }

                // Deduct leave balance based on leave type
                switch (leaveType.ToLower())
                {
                    case "casual leave":
                        if (leaveBalance.CasualLeave >= leaveCount)
                        {
                            leaveBalance.CasualLeave -= leaveCount;
                        }
                        else
                        {
                            throw new Exception("Insufficient Casual Leave Balance.");
                        }
                        break;

                    case "sick leave":
                        if (leaveBalance.SickLeave >= leaveCount)
                        {
                            leaveBalance.SickLeave -= leaveCount;
                        }
                        else
                        {
                            throw new Exception("Insufficient Sick Leave Balance.");
                        }
                        break;

                    case "earned leave":
                        if (leaveBalance.EarnedLeave >= leaveCount)
                        {
                            leaveBalance.EarnedLeave -= leaveCount;
                        }
                        else
            {
                            throw new Exception("Insufficient Earned Leave Balance.");
                        }
                        break;

                    default:
                        throw new Exception("Invalid leave type.");
                }

                // Update leave balance in the database
                leaveBalanceRepository.UpdateAsync(leaveBalance);
                await _UnitOfWork.SaveAsync();


                // Return the EmpId if found, otherwise return null


                // Success handling
                TempData["ToastType"] = "success";  // Success, danger, warning, info
                TempData["ToastMessage"] = "Form Submitted successfully!";

                // Always return a redirect after a successful submission
                return RedirectToAction("LeaveStatus");  // Redirect to the appropriate action/view after success
            }
            else
            {
                return RedirectToAction("LeaveStatus"); // Redirect to the appropriate action/view in case of failure
            }
        }


        #endregion

        #region Leave DashBoard


        public async Task<IActionResult> LeaveDashBoard()
        {
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);
            string EmpId = userDetails.userId;

            // Retrieve employee employment details
            var employementDetailsRepository = _UnitOfWork.GetRepository<EmployementTypeDetail>();
            var employeeDetails = await employementDetailsRepository.GetEmployementTypeDetailsByIdAsync(Convert.ToInt32(EmpId));

            // Check if employee details exist and are not empty
            if (employeeDetails == null || !employeeDetails.Any())
            {
                return NotFound("Employee details not found.");
            }

            // Ensure StartDate is valid before proceeding
            if (!DateTime.TryParse(employeeDetails[0].StartDate, out DateTime startDate))
            {
                return BadRequest("Invalid StartDate format.");
            }

            // Calculate leave summary based on the employee's StartDate
            var leaveSummary = await _leaveDashboardService.CalculateLeavePolicy(startDate);


            // Map values to ViewModel
            var viewModel = new LeaveSummaryViewModel
            {
                CasualLeave = leaveSummary.CasualLeave,
                SickLeave = leaveSummary.SickLeave,
                EarnedLeave = leaveSummary.EarnedLeave,
                EarnedLeaveAccrualRate = leaveSummary.EarnedLeaveAccrualRate,
                CarryForwardLimit = leaveSummary.CarryForwardLimit,
                TotalLeave = leaveSummary.TotalLeave
                //YearsOfService = (DateTime.Now - startDate).TotalDays / 365.25 // Adding Years of Service
            };

            return View(viewModel);
            }


        //Leave Dashboard user leave balance specific

        [HttpGet]
        public async Task<IActionResult> GetLeaveBalance()
        {
            // Retrieve the JWT token from the session
            var jwtTokenFromSession = HttpContext.Session.GetString("JwtToken");

            // Extract user details from the JWT token
            var userDetails = JwtSessionHelper.ExtractSessionData(jwtTokenFromSession);

            // Get Employee ID from the JWT token
            int empId = Convert.ToInt32(userDetails.userId);

            // Fetch leave balance based on EmpId
            var leaveBalance = await _leave.GetLeaveBalanceByEmpId(empId);

            // Return leave balance as JSON
            return Json(leaveBalance);
        }



        #endregion



    }
}
