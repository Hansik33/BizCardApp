using BizCardApp.Models;
using BizCardApp.ViewModels;

namespace BizCardApp.Mappers;

public static class BusinessCardMapper
{
    public static BusinessCard ToEntity(BusinessCardViewModel viewModel) => new()
    {
        Id = viewModel.Id,
        FirstName = viewModel.FirstName,
        LastName = viewModel.LastName,
        Company = viewModel.Company,
        JobTitle = viewModel.JobTitle,
        Phone = viewModel.Phone,
        Email = viewModel.Email,
        Address = viewModel.Address
    };

    public static BusinessCardViewModel ToViewModel(BusinessCard entity) => new()
    {
        Id = entity.Id,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        Company = entity.Company,
        JobTitle = entity.JobTitle,
        Phone = entity.Phone,
        Email = entity.Email,
        Address = entity.Address
    };
}