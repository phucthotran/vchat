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

        static DataAccessExtender()
        {
            //db = new vChatContext();
        }

        public static vChatContext db
        {
            get 
            {
                if (_db == null)
                    _db = new vChatContext();

                _db.Users.Create();
                _db.Question.Create();
                _db.FriendList.Create();
                _db.FriendGroup.Create();

                return _db;
            }
        }

        #region DB OPERATION

        public static IDbModel Get(this IDbModel m, int ID)
        {
            return db.Set(m.GetType()).Find(new Object[] { ID }) as IDbModel;
        }

        public static bool New(this IDbModel m)
        {
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    db.Set(m.GetType()).Add(m);
                    db.SaveChanges();
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
                    //db.ChangeTracker.DetectChanges();
                    
                    db.SaveChanges();
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
                    //db.ChangeTracker.DetectChanges();

                    db.Set(m.GetType()).Remove(m);
                    db.SaveChanges();
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

        public static bool DeactiveAccount(this Users um, int UserID, bool Status)
        {
            Users user_info = db.Users.FirstOrDefault(u => u.UserID == UserID);
            user_info.Deactive = Status;

            bool r = user_info.Update();

            return r;
        }

        public static bool ReponseFriendRequest(this Users um, Users User, Users Friend, FriendGroup Group, bool IsAccepted, bool IsIgnored)
        {
            FriendMap FriendRequest = db.FriendList
                                        .Include(m => m.User)
                                        .Include(m => m.Friend)
                                        .FirstOrDefault(m => m.User.UserID == Friend.UserID && m.Friend.UserID == User.UserID);

            FriendRequest.IsAccepted = IsAccepted;
            FriendRequest.IsIgnored = IsIgnored;
            
            if(IsAccepted){
                FriendMap ConnectFriend = new FriendMap
                {
                    User = User,
                    Friend = Friend,
                    FriendGroup = Group,
                    IsAccepted = true,
                    IsIgnored = false
                };

                ConnectFriend.New();
            }

            bool r = FriendRequest.Update();

            return r;
        }

        public static bool RemoveGroup(this Users mm, FriendGroup Group, bool RemoveContact)
        {
            bool done = false;

            if (RemoveContact)
            {
                //Get all friend in group
                List<FriendMap> friends = db.FriendList.Where(fm => fm.FriendGroup.Equals(Group)).ToList();

                //Remove all contact
                foreach (FriendMap friend in friends)
                {
                    done = friend.Remove();

                    if (done == false)
                        return false;
                }
            }

            //Remove group
            done = Group.Remove();

            return done;
        }

        public static bool MoveContact(this Users um, Users User, Users Friend, FriendGroup NewGroup)
        {
            FriendMap FriendMap = db.FriendList
                .Include(m => m.User)
                .Include(m => m.Friend)
                .Include(m => m.FriendGroup)
                .FirstOrDefault(m => m.User.UserID == User.UserID && m.Friend.UserID == Friend.UserID);

            FriendMap.FriendGroup = NewGroup;

            return FriendMap.Update();
        }

        public static bool RemoveContact(this Users um, Users User, Users Friend)
        {
            FriendMap FriendMap = db.FriendList
                .Include(m => m.User)
                .Include(m => m.Friend)
                .FirstOrDefault(m => m.User.UserID == User.UserID && m.Friend.UserID == Friend.UserID);

            return FriendMap.Remove();
        }

        public static List<Users> FriendRequests(this Users um, int UserID)
        {
            return db.FriendList
                .Include(m => m.User)
                .Include(m => m.Friend)
                .Where(m => m.Friend.UserID == UserID && m.IsAccepted == false && m.IsIgnored == false)
                .Select(m => m.User)
                .ToList();
        }

        public static GroupFriendList FriendList(this Users um, int UserID)
        {            
            List<FriendGroup> Groups = db.FriendGroup
                .Include(g => g.Owner)
                .Where(g => g.Owner.UserID == UserID)
                //.ToList()
                .DistinctList();

            List<FriendMap> FriendMap = db.FriendList
                .Include(m => m.User)
                .Include(m => m.Friend)
                .Include(m => m.FriendGroup)
                .Where(m => m.User.UserID == UserID && m.IsAccepted == true && m.IsIgnored == false)
                //.ToList()
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

            foreach(FriendGroup Group in Groups)
                GroupFriendList.FriendGroups.Add(Group);

            return GroupFriendList;
        }

        /*
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
        */

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
                //.ToList()
                .DistinctList();
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

        #region FRIEND MAP EXTENSIONS METHDO

        public static bool AddFriend(this Users um, Users User, Users Friend, FriendGroup Group)
        {
            db.FriendList.Add(new FriendMap{
                    User = User,
                    Friend = Friend,
                    FriendGroup = Group
                });

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

        #endregion
    }
}