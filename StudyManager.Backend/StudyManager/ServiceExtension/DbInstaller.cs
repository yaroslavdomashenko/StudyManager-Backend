using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyManager.Data;
using StudyManager.Data.Entities;
using StudyManager.Data.Infrastructure;
using StudyManager.Services.Interfaces;
using StudyManager.Services.Services;

namespace StudyManager.ServiceExtension
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"))
            );

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IVisitService, VisitService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IHomeworkService, HomeworkService>();
            services.AddTransient<ICommentsService, CommentsService>();
            services.AddTransient<IChatService, ChatService>();


            services.AddTransient<IRepository<Attachment>, Repository<Attachment>>();
            services.AddTransient<IRepository<Comment>, Repository<Comment>>();
            services.AddTransient<IRepository<Course>, Repository<Course>>();
            services.AddTransient<IRepository<Homework>, Repository<Homework>>();
            services.AddTransient<IRepository<Message>, Repository<Message>>();
            services.AddTransient<IRepository<Notification>, Repository<Notification>>();
            services.AddTransient<IRepository<User>, Repository<User>>();
            services.AddTransient<IRepository<Visit>, Repository<Visit>>();
        }
    }
}
