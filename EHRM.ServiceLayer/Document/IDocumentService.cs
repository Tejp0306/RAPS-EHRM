using EHRM.ServiceLayer.Models;
using EHRM.ViewModel.Document;
using EHRM.ViewModel.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Document
{
    public interface IDocumentService
    {
        Task<Result> SaveDocumentAsync(DocumentViewModel model, string createdBy, string FilePath);
        Task<Result> GetAllDocumentAsync();

        Task<Result> UpdateDocumentAsync(int id, int updatedBy, DocumentViewModel model,string FilePath);
        Task<Result> GetAllDocumentByIdAsync(int id);
    }
}
