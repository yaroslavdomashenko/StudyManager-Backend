using AutoMapper;
using StudyManager.Data.Entities;
using StudyManager.Data.Models;
using StudyManager.Data.Models.Comment;
using StudyManager.Data.Models.Homework;
using StudyManager.Data.Models.Notification;
using StudyManager.Data.Models.User;
using StudyManager.Data.Models.Visit;

namespace StudyManager.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<RegisterModel, User>(); // RegisterModel to User 
            CreateMap<LoginModel, User>();

            CreateMap<User, UserSimpleModel>();
            CreateMap<User, UserModel>();
            CreateMap<User, UserImageUsernameModel>();

            CreateMap<Course, CourseModel>();

            CreateMap<Visit, VisitModel>();
            CreateMap<Visit, UsersVisitModel>();

            CreateMap<Notification, NotificationModel>();

            CreateMap<Homework, HomeworkModel>();
            CreateMap<Attachment, AttachmentModel>();

            CreateMap<Comment, CommentModel>();
        }
    }
}
