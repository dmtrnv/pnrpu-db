﻿using System;
using System.Data;

namespace DbLab3.Misc
{
    public static class DataRecordExtensions
    {
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (var i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}