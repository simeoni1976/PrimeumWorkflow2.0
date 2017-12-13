using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.BusinessCore.BusinessLayer.Helpers
{
    public static class ExecSqlHelper
    {

        /// <summary>
        /// Exécute une requête qui n'attend pas de données de retour.
        /// </summary>
        /// <param name="query">Requete sql</param>
        /// <param name="connection">Connexion à la base</param>
        /// <returns></returns>
        public static async Task ExecuteNonQueryAsync(string query, DbConnection connection)
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
            try
            {
                await connection.OpenAsync();
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    int nbr = await command.ExecuteNonQueryAsync();
                }
            }
            finally
            {
                connection.Close();
            }
        }


        public static async Task<IEnumerable<object[]>> ExecuteReaderAsync(string query, DbConnection connection)
        {
            List<object[]> data = new List<object[]>();
            if (connection.State == ConnectionState.Open)
                connection.Close();
            try
            {
                await connection.OpenAsync();
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        if (reader.FieldCount <= 0)
                            continue;

                        object[] ligne = new object[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                            ligne[i] = reader.GetValue(i);
                        data.Add(ligne);
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return data;
        }

        public static async Task<int> ExecuteNonQueryTransactionAsync(string query, IDbContextTransaction transaction)
        {
            int nbr = 0;

            using (DbCommand command = transaction.GetDbTransaction().Connection.CreateCommand())
            {
                command.CommandText = query;
                command.Transaction = transaction.GetDbTransaction();
                nbr = await command.ExecuteNonQueryAsync();
            }

            return nbr;
        }

    }
}
