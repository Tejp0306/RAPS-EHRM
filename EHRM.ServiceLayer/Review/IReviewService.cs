using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Asset;
using EHRM.ViewModel.Employee;
using EHRM.ViewModel.EmployeeDeclaration;
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

        #region Probation Evaluation
        Task<List<GetAllEmployeeViewModel>> GetEmployeeDetailsByManagerIdAsync(int managerId);
        Task<Result> CreateEvaluationFormAsync(EvaluationQuestion model);
        #endregion

        #region Probation Dashboard
        Task<List<EvaluationQuestion>> GetAllEvaluationDetails();
        #endregion

    }
}
