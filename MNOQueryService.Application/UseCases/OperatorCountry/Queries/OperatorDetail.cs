using MNOQueryService.Application.UseCases.MNOQuerys.ViewModels;
using MediatR;
using MNOQueryService.SharedLibrary.Model.ResponseModel;
using FluentValidation;
using MNOQueryService.Domain.Interfaces;
using MNOQueryService.SharedLibrary.Exceptions;
using MNOQueryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MNOQueryService.Application.UseCases.MNOQuerys.Queries
{

    public class OperatorDetail
    {
        public record Query(string phoneNumber) : IRequest<OperatorResponse>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.phoneNumber)
                     .NotEmpty()
                     .WithMessage("PhoneNumber is required.")
                     .Length(13)
                     .WithMessage("Invalid PhoneNumber.");
            }
        }

        public class QueryHandler : IRequestHandler<Query, OperatorResponse>
        {
            private readonly IMNODbContext mNODbContext;
            private readonly IConfiguration configuration;

            public QueryHandler(IMNODbContext mNODbContext, IConfiguration configuration)
            {
                this.mNODbContext = mNODbContext;
                this.configuration = configuration;
            }

            public async Task<OperatorResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                string countryCode = request.phoneNumber.Substring(0, 3);

                var dbResult = await this.mNODbContext.Countries
                           .Include(c => c.Operators)
                          .FirstOrDefaultAsync(c => c.CountryCode == countryCode, cancellationToken);

                if (dbResult == null)
                {
                    throw new EntityNotFoundException($"Operator details for {request.phoneNumber} does not exist");
                }       
                
                var response = new OperatorResponse
                {
                    Number = request.phoneNumber,
                    Country = new OperatorCountryResponse
                    {
                        CountryCode = dbResult.CountryCode,
                        Name = dbResult.Name,
                        CountryIso = dbResult.CountryIso,
                        CountryDetails = dbResult.Operators.Select(o => new OperatorCountryDetailsResponse
                        {
                            Operator = o.Operator,
                            OperatorCode = o.OperatorCode
                        }).ToList()
                    }
                };

                return response;
            }
        }
    }
}
