using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apache.Arrow.Adbc.Drivers.Apache.Spark;
using cL = Apache.Arrow.Adbc.Client;
using Xunit;
using static Apache.Arrow.Adbc.Tests.Drivers.Apache.Spark.SparkConnectionTest;
using Xunit.Abstractions;

namespace Apache.Arrow.Adbc.Tests.Drivers.Apache.Spark
{
    public class HDInsightTest
    {

        [Fact()]
        public void CanConnect()
        {
            var connectionStringBuilderSparkHDInsight = new DbConnectionStringBuilder();
            connectionStringBuilderSparkHDInsight[SparkParameters.HostName] = "<sparkHost>";
            connectionStringBuilderSparkHDInsight[SparkParameters.Port] = "443";
            connectionStringBuilderSparkHDInsight["username"] = "admin";
            connectionStringBuilderSparkHDInsight["password"] = "<password>";
            connectionStringBuilderSparkHDInsight[SparkParameters.AuthType] = SparkAuthTypeConstants.Basic;
            connectionStringBuilderSparkHDInsight[SparkParameters.Type] = SparkServerTypeConstants.Http;
            connectionStringBuilderSparkHDInsight[SparkParameters.Path] = "/sparkhive2";
            connectionStringBuilderSparkHDInsight[SparkParameters.DataTypeConv] = "none";
            //connectionStringBuilder["adbc.statement.batch_size"] = "1000";
            string connectionStringSparkHDInsight = connectionStringBuilderSparkHDInsight.ConnectionString;

            var connectionStringBuilderHadoopHDInsight = new DbConnectionStringBuilder();
            connectionStringBuilderHadoopHDInsight[SparkParameters.HostName] = "<hadoopHost>";
            connectionStringBuilderHadoopHDInsight[SparkParameters.Port] = "443";
            connectionStringBuilderHadoopHDInsight["username"] = "admin";
            connectionStringBuilderHadoopHDInsight["password"] = "<password>";
            connectionStringBuilderHadoopHDInsight[SparkParameters.AuthType] = SparkAuthTypeConstants.Basic;
            connectionStringBuilderHadoopHDInsight[SparkParameters.Type] = SparkServerTypeConstants.Http;
            connectionStringBuilderHadoopHDInsight[SparkParameters.Path] = "/hive2";
            connectionStringBuilderHadoopHDInsight[SparkParameters.DataTypeConv] = "none";
            string connectionStringHadoopHDInsight = connectionStringBuilderHadoopHDInsight.ConnectionString;

            using (var connection = new cL.AdbcConnection(connectionStringHadoopHDInsight))
            {
                connection.AdbcDriver = new SparkDriver();

                connection.Open();
                Console.WriteLine("Connection to Spark cluster established successfully.");

                cL.AdbcCommand adbcCommand = connection.CreateCommand();

                adbcCommand.CommandText = "SELECT * FROM 10mb";
                var reader = adbcCommand.ExecuteReader();

                int count = 0;
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetValue(i)} ");
                    }
                    Console.WriteLine();
                    count++;
                }

                Console.WriteLine($"\n Total no of records: {count}");
                adbcCommand.CommandText = "SHOW TABLES";
                reader = adbcCommand.ExecuteReader();

                Console.WriteLine("\nListing all tables in default database:\n");

                while (reader.Read())
                {
                    Console.WriteLine($"{reader.GetValue(1)}");
                }



                connection.Close();
                Console.WriteLine("\nConnection to Spark cluster closed.");
                Assert.True(true);
            }
        }
    }
}
