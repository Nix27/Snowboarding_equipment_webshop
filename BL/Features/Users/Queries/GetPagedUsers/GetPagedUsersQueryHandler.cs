using BL.DTOs;
using BL.Features.Users.Queries.GetAllUsers;
using MediatR;

namespace BL.Features.Users.Queries.GetPagedUsers
{
    internal class GetPagedUsersQueryHandler : IRequestHandler<GetPagedUsersQuery, (IEnumerable<UserDto>, int)>
    {
        private readonly IMediator _mediator;

        public GetPagedUsersQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<(IEnumerable<UserDto>, int)> Handle(GetPagedUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _mediator.Send(new GetAllUsersQuery(includeProperties:"Country"));

            if(request.searchTerm != null)
            {
                var searchTerm = request.searchTerm.ToLower();

                if (request.searchBy == "name")
                {
                    users = users.Where(u => u.Name.ToLower().Contains(searchTerm));
                }

                if (request.searchBy == "city")
                {
                    users = users.Where(u => u.City.ToLower().Contains(searchTerm));
                }

                if (request.searchBy == "country")
                {
                    users = users.Where(u => u.Country.Name.ToLower().Contains(searchTerm));
                }
            }

            int numberOfUsers = users.Count();

            var pagedUsers = users.Skip((request.page - 1) * (int)request.size).Take((int)request.size);

            return (pagedUsers, numberOfUsers);
        }
    }
}
