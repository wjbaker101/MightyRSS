﻿using NHibernate.SqlTypes;
using NpgsqlTypes;
using System.Data;

namespace Data.Records.Types;

public sealed class NpgsqlExtendedSqlType : SqlType
{
    public NpgsqlDbType NpgDbType { get; }

    public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType)
        : base(dbType)
    {
        NpgDbType = npgDbType;
    }

    public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, int length)
        : base(dbType, length)
    {
        NpgDbType = npgDbType;
    }

    public NpgsqlExtendedSqlType(DbType dbType, NpgsqlDbType npgDbType, byte precision, byte scale)
        : base(dbType, precision, scale)
    {
        NpgDbType = npgDbType;
    }
}