using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EducaMBAXpert.Api.Configuration
{

    public static class AutoMapperConfig
    {
        public static WebApplicationBuilder AddAutoMapperConfig(this WebApplicationBuilder builder)
        {
            //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return builder;
        }
    }

    //public class AutomapperConfiguration : Profile
    //{
    //    public AutomapperConfiguration()
    //    {
    //        CreateMap<Budget, BudgetViewModel>().ReverseMap();
    //        CreateMap<Category, CategoryViewModel>().ReverseMap();
    //        CreateMap<GeneralBudget, GeneralBudgetViewModel>().ReverseMap();
    //        CreateMap<Transaction, TransactionViewModel>().ReverseMap();
    //        CreateMap<User, UserViewModel>().ReverseMap();
    //    }
    //}

}
