using FluentValidation;
using Vita_WebApi_Shared;

namespace Vita_WebApi_API.Validation;

public class BaseEntityValidation : AbstractValidator<BaseEntity>
{
    public BaseEntityValidation()
    {
        const string idErrorMessage = "Id is required";
        const string userIdErrorMessage = "UserId is required";
        const int maxTitleLength = 200;
        const int minTitleLength = 1;
        const string titleErrorMessage = "Title is required";
        var titleMaxLengthErrorMessage = $"Title must be at max {maxTitleLength} characters";
        var titleMinLengthErrorMessage = $"Title must be at least {minTitleLength} character";
        const string descriptionErrorMessage = "Description is required";
        const int minDescriptionLength = 1;
        const int maxDescriptionLength = 2000;
        var descriptionMinLengthErrorMessage = $"Description must be at least {minDescriptionLength} character";
        var descriptionMaxLengthErrorMessage = $"Description must be at max {maxDescriptionLength} characters";
        
        RuleFor(baseEntity => baseEntity.Id)
            .NotEmpty()
            .WithMessage(idErrorMessage);
            
        RuleFor(baseEntity => baseEntity.UserId)
            .NotEmpty()
            .WithMessage(userIdErrorMessage);
        
        RuleFor(baseEntity => baseEntity.Title)
            .NotEmpty()
            .WithMessage(titleErrorMessage)
            .MinimumLength(minTitleLength)
            .WithMessage(titleMinLengthErrorMessage)
            .MaximumLength(maxTitleLength)
            .WithMessage(titleMaxLengthErrorMessage);

        RuleFor(baseEntity => baseEntity.Description)
            .NotEmpty()
            .WithMessage(descriptionErrorMessage)
            .MinimumLength(minDescriptionLength)
            .WithMessage(descriptionMinLengthErrorMessage)
            .MaximumLength(maxDescriptionLength)
            .WithMessage(descriptionMaxLengthErrorMessage);
        
        RuleFor(baseEntity => baseEntity.CreatedAt)
            .NotEmpty()
            .WithMessage("CreatedAt is required");
        
        RuleFor(baseEntity => baseEntity.UpdatedAt)
            .NotEmpty()
            .WithMessage("UpdatedAt is required");
        
        RuleFor(baseEntity => baseEntity.CreatedBy)
            .NotEmpty()
            .WithMessage("CreatedBy is required");
        
        RuleFor(baseEntity =>  baseEntity.UpdatedBy)
            .NotEmpty()
            .WithMessage("UpdatedBy is required");
        
        

    }
}