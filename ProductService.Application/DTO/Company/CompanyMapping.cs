using ProductService.Application.DTO.Company.Dto;

namespace ProductService.Application.DTO.Company;

public class CompanyMapping
{
    public static Domain.Model.Company ToModel(CompanyDto dto, Guid ownerId)
    {
        var result = new Domain.Model.Company()
        {
            Name = dto.Name,
            OwnerId = ownerId,
            Description = dto.Description,
        };
        return result;
    }
}