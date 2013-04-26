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
        private static vChatContext _db;

        public static vChatContext db
        {
            get 
            {
                //if (_db == null)
                //{
                //    _db = new vChatContext();
                //}
                
                //_db.Users.Create();
                //_db.Question.Create();
                //_db.FriendMap.Create();
                //_db.FriendGroup.Create();

                //return _db;

                return new vChatContext();
            }
        }

        #region DB OPERATION

        //public static IDbModel Get(this IDbModel m, int ID)
        //{
        //    return db.Set(m.GetType()).Find(new Object[] { ID }) as IDbModel;
        //}

        public static bool New(this IDbModel m)
        {
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    vChatContext dbProcess = db;

                    dbProcess.Set(m.GetType()).Attach(m);
                    dbProcess.Entry(m).State = System.Data.EntityState.Added;

                    dbProcess.SaveChanges();
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }

            } while (saveFailed);

            return true;
        }

        public static bool Update(this IDbModel m)
        {
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    vChatContext dbProcess = db;

                    dbProcess.Set(m.GetType()).Attach(m);
                    dbProcess.Entry(m).State = System.Data.EntityState.Modified;

                    dbProcess.SaveChanges();
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }

            } while (saveFailed);

            return true;
        }

        public static bool Remove(this IDbModel m)
        {
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    vChatContext dbProcess = db;

                    dbProcess.Set(m.GetType()).Attach(m);

                    dbProcess.Set(m.GetType()).Remove(m);                   

                    dbProcess.SaveChanges();
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }

            } while (saveFailed);

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

        public static bool AddFriend(this Users um, Users User, Users Friend, FriendGroup Group)
        {
            FriendMap newFriend = new FriendMap
            {
                User = User,
                Friend = Friend,
                FriendGroup = Group
            };

            return newFriend.New();
        }

        public static bool Deactive(this Users um, int UserID, bool Status)
        {
            Users userInfo = db.Users.FirstOrDefault(u => u.UserID == UserID);
            userInfo.Deactive = Status;

            return userInfo.Update();
        }

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

        public static Users GetByName(this Users um, String Username)
        {
            return db.Users.FirstOrDefault(u => u.Username.Equals(Username));
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

        public static bool RemoveGroup(this Users mm, FriendGroup Group, bool RemoveContact)
        {
            bool done = false;

            if (RemoveContact)
            {
                //Get all friend in group
                List<FriendMap> friends = db.FriendMap
                                                      .Where(fm => fm.FriendGroup.Equals(Group))
                                                      .ToList();

                //Remove all contact
                foreach (FriendMap friend in friends)
                {
                    done = friend.Remove();

                    if (!done)
                        return false;
                }
            }

            //Remove group
            done = Group.Remove();

            return done;
        }

        public static bool MoveContact(this Users um, Users User, Users Friend, FriendGroup NewGroup)
        {
            FriendMap FriendMap = db.FriendMap
                .Include(m => m.User)
                .Include(m => m.Friend)
                .Include(m => m.FriendGroup)
                .FirstOrDefault(
                                m => m.User.UserID == User.UserID
                                    && m.Friend.UserID == Friend.UserID
                                );

            FriendMap.FriendGroup = NewGroup;

            return FriendMap.Update();
        }

        public static bool RemoveContact(this Users um, Users User, Users Friend)
        {
            FriendMap FriendMap = db.FriendMap
                .Include(m => m.User)
                .Include(m => m.Friend)
                .FirstOrDefault(
                                m => m.User.UserID == User.UserID
                                    && m.Friend.UserID == Friend.UserID
                                );

            return FriendMap.Remove();
        }

        public static List<Users> Requests(this Users um, int UserID)
        {
            return db.FriendMap
                .Include(m => m.User)
                .Include(m => m.Friend)
                .Where(
                       m => m.Friend.UserID == UserID
                           && m.IsAccepted == false && m.IsIgnored == false
                       )
                .Select(m => m.User)
                .ToList();
        }

        public static bool RequestProcess(this Users um, Users User, Users Friend, FriendGroup Group, bool IsAccepted, bool IsIgnored)
        {
            FriendMap Request = db.FriendMap
                                            .Include(m => m.User)
                                            .Include(m => m.Friend)
                                            .FirstOrDefault(
                                                            m => m.User.UserID == Friend.UserID
                                                                && m.Friend.UserID == User.UserID
                                                            );

            Request.IsAccepted = IsAccepted;
            Request.IsIgnored = IsIgnored;

            bool MapFriendSuccess = false;

            if (IsAccepted)
            {
                FriendMap ConnectFriend = new FriendMap
                {
                    User = User,
                    Friend = Friend,
                    FriendGroup = Group,
                    IsAccepted = true,
                    IsIgnored = false
                };

                MapFriendSuccess = ConnectFriend.New();
            }

            if (IsAccepted && !MapFriendSuccess)
                return false;

            return Request.Update();
        }

        public static GroupFriendList FriendList(this Users um, int UserID)
        {
            List<FriendGroup> Groups = db.FriendGroup
                .Include(g => g.Owner)
                .Where(g => g.Owner.UserID == UserID)
                .DistinctList();

            List<FriendMap> FriendMap = db.FriendMap
                .Include(m => m.User)
                .Include(m => m.Friend)
                .Include(m => m.FriendGroup)
                .Where(
                        m => m.User.UserID == UserID
                            && m.IsAccepted == true && m.IsIgnored == false
                        )
                .DistinctList();

            foreach (FriendGroup Group in Groups)
            {
                List<FriendMap> FriendMapFiltered = FriendMap.Where(fm => fm.FriendGroup.GroupID == Group.GroupID).ToList();
                List<Users> MyFriends = FriendMapFiltered.Select(mf => mf.Friend).ToList();

                foreach (Users friend in MyFriends)
                {
                    if (!Group.Friends.Contains(friend))
                        Group.Friends.Add(friend);
                }
            }

            GroupFriendList GroupFriendList = new GroupFriendList();

            foreach (FriendGroup Group in Groups)
                GroupFriendList.FriendGroups.Add(Group);

            return GroupFriendList;
        }        

        #endregion

        #region FRIENDGROUP EXTENSIONS METHOD

        public static FriendGroup Get(this FriendGroup fg, int GroupID)
        {
            return db.FriendGroup
                .FirstOrDefault(g => g.GroupID == GroupID);
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

        public static List<Conversation> GetByUser(this Conversation conv, int UserID)
        {
            return db.Conversation
                .Include(c => c.SentBy)
                .Include(c => c.SendTo)
                .Where(c => c.SentBy.UserID == UserID || c.SendTo.UserID == UserID)                
                .DistinctList();
        }

        public static List<Conversation> GetNewestByUser(this Conversation conv, int UserID)
        {
            return db.Conversation
                .Include(c => c.SentBy)
                .Include(c => c.SendTo)
                .Where(c => c.SendTo.UserID == UserID && c.IsRead == false)
                .DistinctList();
        }

        #endregion

        #region QUESTION EXTENSIONS METHOD

        public static Question Get(this Question qs, int QuestionID)
        {
            return db.Question
                .FirstOrDefault(q => q.QuestionID == QuestionID);
        }

        public static List<Question> GetAll(this Question q)
        {
            return db.Question.ToList();
        }

        #endregion

        #region FRIEND MAP EXTENSIONS METHDO

        public static FriendMap Get(this FriendMap fm, int FriendMapID)
        {
            return db.FriendMap
                .FirstOrDefault(m => m.FriendMapID == FriendMapID);
        }

        //public static bool Remove(this FriendMap fm)
        //{
        //    db.FriendMap.Attach(fm);
        //    db.Entry(fm).State = System.Data.EntityState.Deleted;

        //    db.FriendMap.Remove(fm);
        //    int i = db.SaveChanges();

        //    db.Entry(fm).State = System.Data.EntityState.Detached;

        //    return true;
        //}

        #endregion
    }
}