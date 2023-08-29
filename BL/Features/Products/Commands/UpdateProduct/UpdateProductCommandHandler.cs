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

namespace BL.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductCommand> _logger;

        public UpdateProductCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UpdateProductCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var productForUpdate = _mapper.Map<Product>(request.productForUpdate);

                _unitOfWork.Product.UpdateAsync(productForUpdate);
                await _unitOfWork.SaveAsync();

                return productForUpdate.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
