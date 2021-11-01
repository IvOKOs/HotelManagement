﻿using System.Collections.Generic;

namespace HotelLibrary.Databases
{
    public interface ISqliteDataAccess
    {
        List<T> LoadData<T, U>(string sqliteStatement, U parameters, string connectionStringName);
        void SaveData<T>(string sqliteStatement, T parameters, string connectionStringName);
    }
}