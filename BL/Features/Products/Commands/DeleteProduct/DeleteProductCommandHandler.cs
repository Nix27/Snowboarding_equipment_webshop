using AutoMapper;
using BL.DTOs;
using BL.Features.Products.Commands.CreateProduct;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteProductCommand> _logger;

        public DeleteProductCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<DeleteProductCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int?> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var productForDelete = _mapper.Map<Product>(request.productForDelete);

                _unitOfWork.Product.Delete(productForDelete);
                await _unitOfWork.SaveAsync();

                return productForDelete.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
