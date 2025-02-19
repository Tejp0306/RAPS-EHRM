using EHRM.DAL.Database;
using EHRM.DAL.UnitOfWork;
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
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> SaveDocumentAsync(DocumentViewModel model, string createdBy, string FilePath)

        {
            try
            {
                var newDocument = new UserDocument
                {
                    DocumentId = model.DocumentId,
                    EmployeeId = model.EmployeeId,
                    DocumentType = model.DocumentType,
                    Description = model.Description,
                    FilePath = FilePath,

                };

                var roleRepository = _unitOfWork.GetRepository<UserDocument>();
                await roleRepository.AddAsync(newDocument);
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Documents created successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error creating Documents: {ex.Message}"
                };
            }

        }

        public async Task<Result> GetAllDocumentByIdAsync(int id)
        {

            try
            {
                var DocumentRepository = _unitOfWork.GetRepository<UserDocument>();  // Using generic repository
                var doc = await DocumentRepository.GetByIdAsync(id);  // Fetch role by ID

                if (doc == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Document not found."
                    };
                }

                var showdoc = new DocumentViewModel
                {
                    DocumentId=doc.DocumentId,
                    FilePath = doc.FilePath,
                    DocumentType = doc.DocumentType,
                    Description = doc.Description,
                    
                };

                return new Result
                {
                    Success = true,
                    Data = showdoc
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error fetching Document: {ex.Message}"
                };
            }
        }


        //Update document

        public async Task<Result> UpdateDocumentAsync(int id, int updatedBy, DocumentViewModel model, string FilePath)
        {
            try
            {
                var documentRepository = _unitOfWork.GetRepository<UserDocument>();  // Using generic repository
                var existingDocument = await documentRepository.GetByIdAsync(id);  // Fetch existing Notice by ID

                if (existingDocument == null)
                {
                    return new Result
                    {
                        Success = false,
                        Message = "Notice not found."
                    };
                }

                // Update NoticeBoard properties
                existingDocument.DocumentType = model.DocumentType;
                existingDocument.Description = model.Description;
                existingDocument.FilePath = FilePath;


                await documentRepository.UpdateAsync(existingDocument);  // Call update method in the generic repository
                await _unitOfWork.SaveAsync();

                return new Result
                {
                    Success = true,
                    Message = "Document updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Success = false,
                    Message = $"Error updating Document: {ex.Message}"
                };
            }
        }

        //Get All Document

        public async Task<Result> GetAllDocumentAsync()
        {
            var documentRepository = _unitOfWork.GetRepository<UserDocument>();  // Using generic repository
            var document = await documentRepository.GetAllAsync();  // Fetch all roles
            return new Result { Success = true, Data = document };
        }
    }


    }
