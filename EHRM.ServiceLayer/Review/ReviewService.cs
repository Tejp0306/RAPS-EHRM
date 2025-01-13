using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
