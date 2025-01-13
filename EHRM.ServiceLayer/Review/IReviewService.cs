using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Master;
using EHRM.ViewModel.Review;


namespace EHRM.ServiceLayer.Review
{
    public interface IReviewService
    {
        Task <Result> GetAllProbationDataAsync();

        

        Task<Result> CreateQuestionAsync(EvaluationQuestion model, string createdBy);

        Task<Result> UpdateQuestionDetailsAsync(int id, EvaluationQuestion model);

        Task<Result> GetQuestionByIdAsync(int id);

        Task<Result> DeleteReviewAsync(int id);

    }
}
