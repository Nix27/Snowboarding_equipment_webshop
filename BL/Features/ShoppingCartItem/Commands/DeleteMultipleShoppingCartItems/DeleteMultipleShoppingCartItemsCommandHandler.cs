using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Features.ShoppingCartItem.Commands.DeleteMultipleShoppingCartItems
{
    internal class DeleteMultipleShoppingCartItemsCommandHandler : IRequestHandler<DeleteMultipleShoppingCartItemsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteMultipleShoppingCartItemsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(DeleteMultipleShoppingCartItemsCommand request, CancellationToken cancellationToken)
        {
            var shoppingCartItemsForDelete = _mapper.Map<IEnumerable<DAL.Models.ShoppingCartItem>>(request.shoppingCartItemsForDelete);
            _unitOfWork.ShoppingCartItem.DeleteMultiple(shoppingCartItemsForDelete);
            await _unitOfWork.SaveAsync();
        }
    }
}
