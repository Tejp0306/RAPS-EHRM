using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.Review;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EHRM.ServiceLayer.Review
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> GetAllProbationDataAsync()
        {
            var probationquesRepository = _unitOfWork.GetRepository<ProbationEvaluationQuestion>();  // Using generic repository
            var ques = await probationquesRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = ques };
        }

        public async Task<Result> CreateQuestionAsync(EvaluationQuestion model, string createdBy)
        {
            try
            {
                var newQuestion = new ProbationEvaluationQuestion
                {
                    Question=model.Question
                    
                };

                var probationRepository = _unitOfWork.GetRepository<ProbationEvaluationQuestion>();
                await probationRepository.AddAsync(newQuestion);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Question Saved successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error saving question: {ex.Message}"
                };
            }
        }

        

        public async Task<Result> UpdateQuestionDetailsAsync(int id, EvaluationQuestion model)
        {
            try
            {
                var probationRepository = _unitOfWork.GetRepository<ProbationEvaluationQuestion>();  // Using generic repository
                var existingQuestion = await probationRepository.GetByIdAsync(id);  // Fetch existing role by ID

                if (existingQuestion == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Question not found."
                    };
                }

                // Update Question properties
                
                existingQuestion.Question = model.Question;

                await probationRepository.UpdateAsync(existingQuestion);  // Call update method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Question updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating Question: {ex.Message}"
                };
            }
        }

        

        public async Task<Result> GetQuestionByIdAsync(int id)
        {
            try
            {
                var quesRepository = _unitOfWork.GetRepository<ProbationEvaluationQuestion>();  // Using generic repository
                var ques = await quesRepository.GetByIdAsync(id);  // Fetch role by ID

                if (ques == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Role not found."
                    };
                }

                var quesViewModel = new EvaluationQuestion
                {
                    Id = ques.QuestionId,
                    Question = ques.Question,
                    
                };

                return new Result
                {
                    Success = true,
                    Data = quesViewModel
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error fetching Question: {ex.Message}"
                };
            }
        }


        public async Task<Result> DeleteReviewAsync(int id)
        {
            try
            {
                var reviewRepository = _unitOfWork.GetRepository<ProbationEvaluationQuestion>();  // Using generic repository
                var QuestionDelete = await reviewRepository.GetByIdAsync(id);  // Fetch question by ID

                if (QuestionDelete == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Notice not found."
                    };
                }

                // Perform hard delete
                await reviewRepository.DeleteAsync(id);  // Call delete method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Question deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error deleting Question: {ex.Message}"
                };
            }
        }



        #region Probation Evaluation
       
        public async Task<List<GetAllEmployeeViewModel>> GetEmployeeDetailsByManagerIdAsync(int managerId)
        {
            try
            {
                var employementTypeRepository = _unitOfWork.GetRepository<EmployementTypeDetail>();
                var employeeRepository = _unitOfWork.GetRepository<EmployeeDetail>();
                var employmentTypeDetails = await employementTypeRepository.GetAllAsync();
                var empDEtails = await employeeRepository.GetAllAsync();
                var empIDs = employmentTypeDetails
                    .Where(x => x.ManagerId == managerId)
                    .Select(x => x.EmpId)
                    .ToList();
                var commonEmpIds = empDEtails
                    .Where(e => empIDs.Contains(e.EmpId))
                    .ToList();
                var commonDetails = commonEmpIds
                    .Select(e => new GetAllEmployeeViewModel
                    {
                        EmpId = e.EmpId,
                        FirstName = e.FirstName + " " + e.LastName  
                    })
                    .ToList();

                return commonDetails;
            }
            catch (Exception ex)
            {
                
                throw new Exception($"An error occurred while retrieving employee details: {ex.Message}");
            }
        }

        public async Task<Result> CreateEvaluationFormAsync(EvaluationQuestion model)
        {
            try
            {
                if(model.Items.Count == 0)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Please fill Remarks"
                    };
                }
                else
                {
                    foreach (var item in model.Items) {
                        var newForm = new ProbationEvaluationForm
                        {
                            EmpId = model.EmpId,
                            ApplicationDate = model.ApplicationDate,
                            ManagerId = model.ManagerId,
                            QuestionId = item.QuestionId,
                            Evaluation1stMonth = item.Evaluation1stMonth,
                            Evaluation2ndMonth = item.Evaluation2ndMonth,
                            Evaluation3rdMonth = item.Evaluation3rdMonth,
                            Recommendation = model.Recommendation,
                            RemarksConfirmation = model.RemarksConfirmation,
                            ManagerSignature = model.ManagerSignature,
                            FinalDate = model.FinalDate,

                        };

                        var FormRepository = _unitOfWork.GetRepository<ProbationEvaluationForm>();
                        await FormRepository.AddAsync(newForm);
                        await _unitOfWork.SaveAsync();
                    }
                    return new Result
                    {
                        Success = true,
                        Message = "Record saved successfully."
                    };
                }
                
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error saving Record: {ex.Message}"
                };
            }
        }


        #endregion


        #region Probation Dashboard

        public async Task<List<EvaluationQuestion>> GetAllEvaluationDetails()
        {
            var employeeRepository = _unitOfWork.GetRepository<ProbationEvaluationForm>();
            var employmentTypeRepository = _unitOfWork.GetRepository<EmployeeDetail>();

            // Await the async operations to get actual collections
            var employees = await employeeRepository.GetAllAsync();
            var employmentTypes = await employmentTypeRepository.GetAllAsync();

            // LINQ query to join EmployeeDetails and ProbationEvaluationForm using EmpId
            var employeeWithDetails = (from e in employmentTypes
                                       join p in employees on e.EmpId equals p.EmpId
                                       select new EvaluationQuestion
                                       {
                                           Id = e.Id,
                                           Recommendation = p.Recommendation,
                                           RemarksConfirmation = p.RemarksConfirmation,
                                           Details = new EmployeeViewModel
                                           {
                                               FirstName = e.FirstName,
                                               LastName = e.LastName
                                           }
                                       }).ToList();

            return employeeWithDetails;
        }
        #endregion
    }
}
