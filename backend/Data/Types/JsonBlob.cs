using Newtonsoft.Json;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;
using System.Data.Common;

namespace MightyRSS.Data.Types
{
    [Serializable]
    public class JsonBlob<T> : IUserType where T : class
    {
        public new bool Equals(object x, object y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            var jsonX = JsonConvert.SerializeObject(x);
            var jsonY = JsonConvert.SerializeObject(y);

            return jsonX == jsonY;
        }

        public int GetHashCode(object x)
        {
            return x == null ? 0 : x.GetHashCode();
        }

        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            if (names.Length > 1)
                throw new InvalidOperationException("Only expected 1 column.");

            if (rs[names[0]] is string value && !string.IsNullOrWhiteSpace(value))
                return JsonConvert.DeserializeObject<T>(value);

            return null;
        }

        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            var parameter = (NpgsqlParameter)cmd.Parameters[index];
            parameter.NpgsqlDbType = NpgsqlDbType.Json;

            if (value == null)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = JsonConvert.SerializeObject(value);
        }

        public object DeepCopy(object value)
        {
            if (value == null)
                return null;

            var json = JsonConvert.SerializeObject(value);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            if (cached is string json && !string.IsNullOrWhiteSpace(json))
                return JsonConvert.DeserializeObject<T>(json);

            return null;
        }

        public object Disassemble(object value)
        {
            return value == null ? null : JsonConvert.SerializeObject(value);
        }

        public SqlType[] SqlTypes
        {
            get
            {
                return new SqlType[]
                {
                    new NpgsqlExtendedSqlType(DbType.Object, NpgsqlDbType.Json)
                };
            }
        }

        public Type ReturnedType => typeof(T);

        public bool IsMutable => true;
    }
}