using System;
using System.Collections.Generic;
using System.Linq;
using vChat.Model;
using vChat.Model.Entities;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;

namespace vChat.Data
{
    public static class ExtensionsMethod
    {
        private static vChatContext db;

        static ExtensionsMethod()
        {
            db = new vChatContext();
        }

        #region DB OPERATION

        public static IDbModel Get(this IDbModel m, int ID)
        {
            return db.Set(m.GetType()).Find(new Object[] { ID }) as IDbModel;
        }

        public static bool New(this IDbModel m)
        {
            try
            {                
                db.Set(m.GetType()).Add(m);
                db.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            return true;
        }

        public static bool Update(this IDbModel m)
        {
            try
            {
                db.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            return true;
        }

        public static bool Remove(this IDbModel m)
        {
            try
            {
                db.Set(m.GetType()).Add(m);
                db.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region INCLUDE() METHOD EXETENSIONS

        public static DbQuery<TEntity> Include<TEntity, TProperty>(this DbQuery<TEntity> query, Expression<Func<TEntity, TProperty>> expression) where TEntity : class
        {
            string name = expression.GetPropertyName();
            return query.Include(name);
        }

        public static string GetPropertyName<TObject, TProperty>(this Expression<Func<TObject, TProperty>> expression) where TObject : class
        {
            if (expression.Body.NodeType == ExpressionType.Call)
            {
                MethodCallExpression methodCallExpression = (MethodCallExpression)expression.Body;
                string name = GetPropertyName(methodCallExpression);
                return name.Substring(expression.Parameters[0].Name.Length + 1);
            }
            return expression.Body.ToString().Substring(expression.Parameters[0].Name.Length + 1);
        }

        private static string GetPropertyName(MethodCallExpression expression)
        {
            MethodCallExpression methodCallExpression = expression.Object as MethodCallExpression;
            if (methodCallExpression != null)
            {
                return GetPropertyName(methodCallExpression);
            }

            return expression.Object.ToString();
        }

        #endregion

        #region USER EXTENSIONS METHOD

        public static Users Get(this Users um, int UserID)
        {
            return db.Users
                .Include(u => u.Question)
                .Include(u => u.SentMessage)
                .Include(u => u.ReceivedMessage)
                .Include(u => u.Group)
                .FirstOrDefault(u => u.UserID == UserID);
        }

        public static List<Users> GetAll(this Users um)
        {
            return db.Users
                .Include(u => u.Question)
                .Include(u => u.SentMessage)
                .Include(u => u.ReceivedMessage)
                .Include(u => u.Group)
                .ToList();
        }

        public static bool IsAvailable(this Users um, String Username, String Password)
        {
            return db.Users.SingleOrDefault(u => u.Username.Equals(Username) && u.Password.Equals(Password)) != null;
        }

        public static bool IsExist(this Users um, String Username)
        {
            return db.Users.SingleOrDefault(u => u.Username.Equals(Username)) != null;
        }

        public static bool DeactiveAccount(this Users um, int UserID, bool Status)
        {
            Users user_info = db.Users.SingleOrDefault(u => u.UserID == UserID);
            user_info.Deactive = Status;

            bool r = user_info.Update();

            return r;
        }

        public static List<Users> FriendList(this Users um, int UserID)
        {
            List<FriendGroup> friendGroup = db.Users.Include(u => u.Question)
                .Include(u => u.SentMessage)
                .Include(u => u.ReceivedMessage)
                .Include(u => u.Group)
                .Include(u => u.Group.Owner)
                .Where(u => u.Group.Owner.UserID == UserID)                
                .Select(u => u.Group)                
                .ToList();

            List<Users> allUsers = um.GetAll();

            List<Users> friends = new List<Users>();

            foreach (Users user in allUsers)
            {
                foreach (FriendGroup group in friendGroup)
                {
                    if (user.Group.GroupID == group.GroupID)
                        friends.Add(user);
                }
            }

            return friends;
        }

        #endregion

        #region CONVERSATION EXTENSIONS METHOD

        public static Conversation Get(this Conversation conv, int ConversationID)
        {
            return db.Conversation
                .Include(c => c.SentBy)
                .Include(c => c.SendTo)
                .FirstOrDefault(c => c.ConversationID == ConversationID);
        }

        public static List<Conversation> GetAll(this Conversation conv)
        {
            return db.Conversation
                .Include(c => c.SentBy)
                .Include(c => c.SendTo)
                .ToList();
        }

        public static List<Conversation> GetConversations(this Conversation conv, int UserID)
        {
            return db.Conversation.Where(c => c.SentBy.UserID == UserID || c.SendTo.UserID == UserID).ToList();
        }

        public static List<Conversation> GetNewestConversations(this Conversation conv, int UserID)
        {
            return db.Conversation.Where(c => c.SendTo.UserID == UserID && c.IsRead == false).ToList();
        }

        #endregion

        #region QUESTION EXTENSIONS METHOD

        //public static Question Get(this Question qes, int QuestionID)
        //{
        //    return db.Question.SingleOrDefault(q => q.QuestionID == QuestionID);
        //}

        public static List<Question> GetAll(this Question q)
        {
            return db.Question.ToList();
        }

        #endregion
    }
}