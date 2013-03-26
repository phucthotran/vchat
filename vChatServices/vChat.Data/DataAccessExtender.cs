using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using vChat.Model;
using vChat.Model.Entities;

namespace vChat.Data
{
    public static class DataAccessExtender
    {
        private static vChatContext db;

        static DataAccessExtender()
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
                db.Set(m.GetType()).Remove(m);
                db.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region ANOTHER LINQ EXTENSIONS METHOD

        public static List<TEntity> DistinctList<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            List<TEntity> lstCompare = query.ToList();

            int maxCompare = lstCompare.Count;
            int beginPosIdx = 0;
            int comparePosIdx = maxCompare - 1;

            while (beginPosIdx < maxCompare)
            {
                TEntity compareEntity = lstCompare.ElementAt(beginPosIdx);

                while (comparePosIdx > beginPosIdx)
                {
                    TEntity beCompare = lstCompare.ElementAt(comparePosIdx);

                    if (compareEntity.Equals(beCompare))
                        lstCompare.RemoveAt(comparePosIdx);

                    comparePosIdx--;
                }

                maxCompare = lstCompare.Count;
                comparePosIdx = maxCompare - 1;
                beginPosIdx++;
            }

            //for (int i = 0; i < lstCompare.Count; i++)
            //{
            //    TEntity compareEntity = lstCompare.ElementAt(i);

            //    for (int j = lstCompare.Count - 1; j > i; j--)
            //    {
            //        TEntity toBeCompareEntity = lstCompare.ElementAt(j);

            //        if (compareEntity.Equals(toBeCompareEntity))
            //        {
            //            lstCompare.RemoveAt(j);                        
            //        }
            //    }
            //}

            return lstCompare;
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
                //.Include(u => u.Question)
                //.Include(u => u.SentMessage)
                //.Include(u => u.ReceivedMessage)
                //.Include(u => u.FriendsFake)
                //.Include(u => u.Friends)
                .FirstOrDefault(u => u.UserID == UserID);
        }

        public static List<Users> GetAll(this Users um)
        {
            return db.Users
                //.Include(u => u.Question)
                //.Include(u => u.SentMessage)
                //.Include(u => u.ReceivedMessage)
                //.Include(u => u.Friends)
                .ToList();
        }

        public static bool IsAvailable(this Users um, String Username, String Password)
        {
            return db.Users
                .FirstOrDefault(u => u.Username.Equals(Username) && u.Password.Equals(Password)) != null;
        }

        public static bool IsExist(this Users um, String Username)
        {
            return db.Users
                .FirstOrDefault(u => u.Username.Equals(Username)) != null;
        }

        public static bool DeactiveAccount(this Users um, int UserID, bool Status)
        {
            Users user_info = db.Users.FirstOrDefault(u => u.UserID == UserID);
            user_info.Deactive = Status;

            bool r = user_info.Update();

            return r;
        }

        public static List<Users> FriendList(this Users um, int UserID)
        {
            return db.FriendList
                .Include(f => f.User)
                //.Include(f => f.Friend)
                //.Include(f => f.FriendGroup)
                .Where(f => f.Friend.UserID == UserID)
                .DistinctList()
                .Select(f => f.User)
                .ToList();
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
            return db.Conversation
                .Include(c => c.SentBy)
                .Include(c => c.SendTo)
                .Where(c => c.SentBy.UserID == UserID || c.SendTo.UserID == UserID)
                .DistinctList()
                .ToList();
        }

        public static List<Conversation> GetNewestConversations(this Conversation conv, int UserID)
        {
            return db.Conversation
                .Include(c => c.SentBy)
                .Include(c => c.SendTo)
                .Where(c => c.SendTo.UserID == UserID && c.IsRead == false)
                .ToList();
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