using AutoMapper;
using BL.DTOs;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.Products.Commands.CreateProduct
{
    public class CeateProductCommandHandler : IRequestHandler<CreateProductCommand, int?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductCommand> _logger;

        public CeateProductCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            ILogger<CreateProductCommand> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newProduct = _mapper.Map<Product>(request.newProduct);

                await _unitOfWork.Product.CreateAsync(newProduct);
                await _unitOfWork.SaveAsync();

                return newProduct.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                return null;
            }
        }
    }
}
